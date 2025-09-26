# Terraform Infrastructure for Smart Mechanical Workshop

This directory contains the complete Infrastructure as Code (IaC) setup for the Smart Mechanical Workshop application, supporting both local development with LocalStack and production deployment on AWS.

## 📋 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Directory Structure](#directory-structure)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Environment Management](#environment-management)
- [Security Best Practices](#security-best-practices)
- [Monitoring and Logging](#monitoring-and-logging)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)

## 🏗️ Architecture Overview

### Multi-Environment Setup

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   LocalStack    │    │   AWS Staging   │    │ AWS Production  │
│   (Development) │    │                 │    │                 │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ • EKS Cluster   │    │ • EKS Cluster   │    │ • EKS Cluster   │
│ • RDS MySQL     │    │ • RDS MySQL     │    │ • RDS MySQL     │
│ • VPC/Networking│    │ • VPC/Networking│    │ • VPC/Networking│
│ • Load Balancers│    │ • Load Balancers│    │ • Load Balancers│
│ • Auto Scaling  │    │ • Auto Scaling  │    │ • Auto Scaling  │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### Components

- **VPC**: Secure network isolation with public and private subnets
- **EKS**: Managed Kubernetes cluster for container orchestration
- **RDS**: MySQL database with automated backups and encryption
- **ALB**: Application Load Balancer with SSL termination
- **ASG**: Auto Scaling Groups for high availability
- **IAM**: Fine-grained security roles and policies
- **KMS**: Encryption key management
- **Secrets Manager**: Secure credential storage

## 📁 Directory Structure

```
terraform/
├── environments/
│   ├── local/                    # LocalStack development
│   │   ├── main.tf              # Main configuration
│   │   ├── variables.tf         # Input variables
│   │   ├── terraform.tfvars     # Variable values
│   │   └── versions.tf          # Provider configurations
│   ├── staging/                 # AWS staging environment
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   ├── terraform.tfvars.example
│   │   └── versions.tf
│   └── production/              # AWS production environment
│       ├── main.tf
│       ├── variables.tf
│       ├── terraform.tfvars.example
│       └── versions.tf
├── modules/
│   ├── vpc/                     # VPC and networking
│   │   ├── main.tf
│   │   ├── outputs.tf
│   │   └── variables.tf
│   ├── rds/                     # RDS MySQL database
│   │   ├── main.tf
│   │   ├── outputs.tf
│   │   └── variables.tf
│   ├── eks/                     # EKS cluster and node groups
│   │   ├── main.tf
│   │   ├── outputs.tf
│   │   └── variables.tf
│   └── security/                # Security (KMS, IAM, Secrets)
│       ├── main.tf
│       ├── outputs.tf
│       └── variables.tf
├── scripts/
│   ├── deploy-local.sh          # LocalStack deployment script
│   ├── deploy-aws.sh           # AWS deployment script
│   ├── destroy-local.sh        # Local environment cleanup
│   └── utils.sh                # Utility functions
├── Makefile                     # Automated commands
└── README.md                    # This file
```

## 🔧 Prerequisites

### Required Software

| Tool | Version | Purpose |
|------|---------|---------|
| **Terraform** | >= 1.0 | Infrastructure provisioning |
| **AWS CLI** | >= 2.0 | AWS resource management |
| **kubectl** | >= 1.25 | Kubernetes management |
| **Helm** | >= 3.0 | Kubernetes package management |
| **Docker** | >= 20.10 | LocalStack container runtime |
| **LocalStack** | >= 2.0 | Local AWS simulation |
| **jq** | latest | JSON processing |

### Installation Commands

```bash
# macOS with Homebrew
brew install terraform awscli kubectl helm docker jq
pip install localstack

# Ubuntu/Debian
curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -
sudo apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main"
sudo apt-get update && sudo apt-get install terraform

# Install other tools...
```

### AWS Configuration

```bash
# Configure AWS credentials
aws configure

# Verify access
aws sts get-caller-identity
```

## 🚀 Quick Start

### 1. Local Development (LocalStack)

```bash
# Start LocalStack
localstack start

# Deploy local environment
make local-deploy

# Check status
make local-status

# Access applications via port-forward
kubectl port-forward svc/dev-api-service 5180:5180 -n smart-mechanical-workshop-dev

# Clean up when done
make local-destroy
```

### 2. AWS Staging Deployment

```bash
# Plan deployment (dry-run)
make aws-plan-staging

# Deploy to staging
export TF_VAR_db_password="your-secure-password"
make aws-deploy-staging

# Verify deployment
kubectl get all -n smart-mechanical-workshop-staging
```

### 3. AWS Production Deployment

```bash
# Set required environment variables
export TF_VAR_db_password="your-production-password"
export TF_VAR_domain_name="workshop.yourcompany.com"
export TF_VAR_ssl_certificate_arn="arn:aws:acm:us-east-1:123456789:certificate/xxx"

# Plan deployment
make aws-plan-production

# Deploy to production (requires confirmation)
make aws-deploy-production
```

## 🌍 Environment Management

### Environment Comparison

| Feature | Local | Staging | Production |
|---------|-------|---------|------------|
| **Infrastructure** | LocalStack | AWS | AWS |
| **EKS Nodes** | 1 x t3.small | 2 x m5.large | 3 x m5.xlarge |
| **RDS Instance** | db.t3.micro | db.r6g.medium | db.r6g.large |
| **Multi-AZ** | No | No | Yes |
| **Backup Retention** | 0 days | 7 days | 30 days |
| **SSL/TLS** | No | Optional | Required |
| **Monitoring** | Basic | Enhanced | Full |
| **Cost** | Free | Low | Production |

### Configuration Management

Each environment uses its own configuration:

```bash
# Local environment variables
terraform/environments/local/terraform.tfvars

# Staging environment variables
terraform/environments/staging/terraform.tfvars

# Production environment variables
terraform/environments/production/terraform.tfvars
```

### Terraform Backend Configuration

- **Local**: Local state file (no remote backend)
- **Staging**: S3 backend with DynamoDB locking
- **Production**: S3 backend with DynamoDB locking (separate bucket)

## 🔒 Security Best Practices

### 1. Credential Management

```bash
# Use environment variables for sensitive data
export TF_VAR_db_password="$(openssl rand -base64 32)"

# Or use AWS Secrets Manager
aws secretsmanager create-secret \
    --name "workshop/production/db-password" \
    --secret-string "your-secure-password"
```

### 2. Network Security

- **Private Subnets**: Database and application nodes in private subnets
- **Security Groups**: Least-privilege access rules
- **NACLs**: Additional network-level security
- **VPC Endpoints**: Secure AWS service access

### 3. Encryption

- **At Rest**: RDS encryption, EBS volume encryption
- **In Transit**: TLS for all communications
- **KMS**: Customer-managed encryption keys
- **Secrets**: AWS Secrets Manager integration

### 4. IAM Security

- **IRSA**: IAM Roles for Service Accounts
- **Least Privilege**: Minimal required permissions
- **MFA**: Multi-factor authentication for human users
- **Rotation**: Regular credential rotation

## 📊 Monitoring and Logging

### Built-in Monitoring

```bash
# CloudWatch metrics and logs are automatically configured
# EKS cluster logging enabled for:
# - API server
# - Audit
# - Authenticator
# - Controller manager
# - Scheduler
```

### Additional Monitoring (Optional)

Deploy monitoring stack:

```bash
# Prometheus and Grafana
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm install monitoring prometheus-community/kube-prometheus-stack

# AWS Load Balancer Controller
# Automatically deployed in production environment

# Cluster Autoscaler
# Automatically configured for all environments
```

### Log Aggregation

```bash
# Fluent Bit for log shipping
kubectl apply -f https://raw.githubusercontent.com/aws/aws-for-fluent-bit/mainline/examples/fluent-bit-daemonset.yaml

# View logs in CloudWatch
aws logs describe-log-groups --log-group-name-prefix "/aws/eks/smart-mechanical-workshop"
```

## 🔧 Troubleshooting

### Common Issues

#### 1. LocalStack Connection Issues

```bash
# Check LocalStack status
curl -s http://localhost:4566/health | jq

# Restart LocalStack
localstack stop
localstack start

# Debug LocalStack logs
localstack logs
```

#### 2. AWS Authentication Issues

```bash
# Check AWS credentials
aws sts get-caller-identity

# Configure AWS profile
aws configure --profile workshop

# Use specific profile
export AWS_PROFILE=workshop
```

#### 3. Terraform State Issues

```bash
# Local state corruption
rm -rf .terraform terraform.tfstate*
terraform init

# Remote state locking
terraform force-unlock LOCK_ID

# Import existing resources
terraform import aws_instance.example i-1234567890abcdef0
```

#### 4. Kubernetes Access Issues

```bash
# Update kubeconfig
aws eks update-kubeconfig --name smart-mechanical-workshop-staging

# Check cluster status
kubectl cluster-info

# Debug node issues
kubectl describe nodes
kubectl get events --sort-by='.lastTimestamp'
```

#### 5. RDS Connection Issues

```bash
# Check security groups
aws ec2 describe-security-groups --group-ids sg-xxx

# Test database connectivity from pod
kubectl run mysql-test --rm -it --image=mysql:8.0 -- mysql -h <endpoint> -u <user> -p

# Check RDS logs
aws rds describe-db-log-files --db-instance-identifier smart-mechanical-workshop-production-db
```

### Debug Commands

```bash
# Local environment debug
make debug-local

# AWS environment debug
make debug-aws

# Terraform debug mode
export TF_LOG=DEBUG
terraform apply

# Kubernetes debug
kubectl get events --all-namespaces --sort-by='.lastTimestamp'
kubectl describe pod <pod-name> -n <namespace>
```

### Recovery Procedures

#### 1. Infrastructure Recovery

```bash
# Backup current state
cp terraform.tfstate terraform.tfstate.backup

# Plan disaster recovery
terraform plan -destroy

# Selective resource recreation
terraform taint aws_instance.example
terraform apply
```

#### 2. Database Recovery

```bash
# Point-in-time recovery
aws rds restore-db-instance-to-point-in-time \
    --source-db-instance-identifier smart-mechanical-workshop-prod-db \
    --target-db-instance-identifier smart-mechanical-workshop-prod-db-restored \
    --restore-time 2023-12-01T12:00:00.000Z
```

## 💰 Cost Optimization

### Resource Sizing

```bash
# Development: ~$50-100/month
# Staging: ~$200-400/month
# Production: ~$500-1000/month (depending on usage)
```

### Cost Reduction Tips

1. **Use Spot Instances** for non-production workloads
2. **Schedule Start/Stop** for development environments
3. **Right-size Resources** based on actual usage
4. **Use Reserved Instances** for production workloads
5. **Enable Cost Allocation Tags** for tracking

## 🤝 Contributing

### Development Workflow

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/new-infrastructure-component
   ```

2. **Make Changes**
   ```bash
   # Edit Terraform files
   # Test locally with LocalStack
   make local-deploy
   ```

3. **Validate and Format**
   ```bash
   make validate
   make format
   make lint
   ```

4. **Test in Staging**
   ```bash
   make aws-plan-staging
   make aws-deploy-staging
   ```

5. **Submit Pull Request**

### Code Standards

- **Terraform Format**: Use `terraform fmt`
- **Variable Naming**: Use snake_case
- **Resource Naming**: Include environment and project prefixes
- **Documentation**: Update README.md for significant changes
- **Tagging**: Apply consistent resource tags

### Testing

```bash
# Local testing
make local-deploy
# Run integration tests
# Clean up
make local-destroy

# Staging testing
make aws-deploy-staging
# Run end-to-end tests
# Verify functionality
```

## 📚 Additional Resources

- [Terraform Best Practices](https://www.terraform-best-practices.com/)
- [AWS EKS Best Practices](https://aws.github.io/aws-eks-best-practices/)
- [Kubernetes Security Best Practices](https://kubernetes.io/docs/concepts/security/)
- [LocalStack Documentation](https://docs.localstack.cloud/)

## 🆘 Support

### Getting Help

1. **Check Documentation**: Review this README and module documentation
2. **Search Issues**: Check existing GitHub issues
3. **Debug Locally**: Use LocalStack for safe testing
4. **Create Issue**: Provide detailed information and logs

### Emergency Contacts

- **Infrastructure Team**: infrastructure@yourcompany.com
- **DevOps Team**: devops@yourcompany.com
- **On-Call**: +1-555-ON-CALL

---

**Developed by**: FIAP SOAT Team
**Project**: Smart Mechanical Workshop
**Version**: 1.0.0
**Last Updated**: December 2024
