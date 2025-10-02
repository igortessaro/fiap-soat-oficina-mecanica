# Terraform Infrastructure - Smart Mechanical Workshop

Configuração **acadêmica** simplificada e econômica para projeto educacional.

## 📋 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Commands](#commands)
- [What Will Be Created](#what-will-be-created)
- [Next Steps](#next-steps)
- [Troubleshooting](#troubleshooting)

## 🏗️ Architecture Overview

### Single Production Environment

```text
┌─────────────────────────────────────┐
│           AWS Production            │
├─────────────────────────────────────┤
│ • VPC com subnets públicas/privadas │
│ • EKS Cluster (Kubernetes)          │
│ • RDS MySQL                         │
│ • Security Groups                   │
│ • IAM Roles                         │
└─────────────────────────────────────┘
```

### Components

- **VPC**: Secure network isolation with public and private subnets
- **EKS**: Managed Kubernetes cluster for container orchestration
- **RDS**: MySQL database with automated backups and encryption
- **Security Groups**: Network security rules
- **IAM**: Roles and policies for EKS and applications

## 📁 Directory Structure

```text
terraform/
├── environments/
│   └── production/              # AWS production environment
│       ├── main.tf
│       ├── variables.tf
│       ├── terraform.tfvars
│       └── versions.tf
├── modules/
│   ├── vpc/                     # VPC and networking
│   ├── rds/                     # RDS MySQL database
│   ├── eks/                     # EKS cluster and node groups
│   └── security/                # Security (IAM)
├── scripts/
│   └── utils.sh                # Utility functions
├── Makefile                     # Automated commands
└── README.md                    # This file
```

## 🔧 Prerequisites

### Required Software

| Tool           | Version  | Purpose                       |
| -------------- | -------- | ----------------------------- |
| **Terraform**  | >= 1.0   | Infrastructure provisioning   |
| **AWS CLI**    | >= 2.0   | AWS resource management       |
| **kubectl**    | >= 1.25  | Kubernetes management         |
| **jq**         | latest   | JSON processing               |

### Installation Commands

```bash
# macOS with Homebrew
brew install terraform awscli kubectl jq

# Ubuntu/Debian
curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -
sudo apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main"
sudo apt-get update && sudo apt-get install terraform
```

### AWS Configuration

```bash
# Configure AWS credentials
aws configure

# Verify access
aws sts get-caller-identity
```

## 🚀 Quick Start

### 1. Configure Database Password

```bash
# Generate a secure password
export TF_VAR_db_password="$(openssl rand -base64 32)"

# Or set your own password
export TF_VAR_db_password="your-secure-password"
```

### 2. Plan Deployment (Dry-run)

```bash
# See what will be created
make plan
```

### 3. Deploy to Production

```bash
# Deploy infrastructure
make deploy
```

### 4. Configure kubectl

```bash
# Update kubeconfig to access the EKS cluster
aws eks update-kubeconfig --name smart-mechanical-workshop-production
```

### 5. Verify Deployment

```bash
# Check cluster status
kubectl cluster-info

# Check nodes
kubectl get nodes

# Check production status
make status
```

## 📋 Commands

All commands are available through the Makefile:

```bash
make help      # Show available commands
make plan      # Plan deployment (dry-run)
make deploy    # Deploy to production
make status    # Show current status
make output    # Show Terraform outputs
make destroy   # Destroy all resources (CAREFUL!)
make format    # Format Terraform files
make validate  # Validate configuration
make lint      # Check code formatting
make clean     # Clean up temporary files
```

## 🔧 What Will Be Created

### Network Infrastructure

- **VPC** with CIDR 10.0.0.0/16
- **Public Subnets** in 2 availability zones (us-east-1a, us-east-1b)
- **Private Subnets** in 2 availability zones
- **Internet Gateway** for public internet access
- **NAT Gateways** for private subnet internet access
- **Route Tables** and proper routing

### EKS Cluster

- **EKS Cluster** version 1.28
- **Node Group** with 1-3 t3.small instances (econômico)
- **Auto Scaling** enabled
- **IAM Roles** for cluster and nodes
- **Security Groups** for secure communication

### Database

- **RDS MySQL** db.t3.micro instance (menor disponível)
- **20GB** allocated storage (suficiente para testes)
- **Automated backups** disabled (economia)
- **Single AZ** deployment (mais barato)
- **Private subnet** placement
- **Security Groups** restricting access

### Security

- **IAM Roles** with least-privilege access
- **Security Groups** with minimal required ports
- **Private networking** for database and worker nodes
- **Encrypted storage** for all components

## 💰 Estimated Costs (Configuração Acadêmica)

```text
Monthly costs (approximate):
- EKS Cluster: ~$75/month
- EC2 Nodes (1 x t3.small): ~$15/month
- RDS MySQL (db.t3.micro): ~$15/month
- Networking (NAT Gateway): ~$45/month
- Storage (20GB): ~$2/month

Total: ~$150/month
```

> **💡 Configuração Acadêmica**: Esta configuração prioriza custo baixo sobre alta disponibilidade. Para produção real, considere usar instâncias maiores, multi-AZ, backups automáticos e monitoramento robusto.

## 🎯 Next Steps (Future Versions)

- [ ] Add SSL/TLS certificate and custom domain
- [ ] Implement Application Load Balancer
- [ ] Add monitoring with CloudWatch and Prometheus
- [ ] Create staging environment
- [ ] Implement automated backups and disaster recovery
- [ ] Add CI/CD pipeline integration
- [ ] Implement secrets management with AWS Secrets Manager

## 🔧 Troubleshooting

### Common Issues

#### 1. AWS Authentication Issues

```bash
# Check your AWS credentials
aws sts get-caller-identity

# Configure AWS profile if needed
aws configure --profile workshop
export AWS_PROFILE=workshop
```

#### 2. Terraform State Issues

```bash
# If you get state locking issues
terraform force-unlock LOCK_ID

# If state is corrupted
rm -rf .terraform terraform.tfstate*
terraform init
```

#### 3. EKS Access Issues

```bash
# Update kubeconfig
aws eks update-kubeconfig --name smart-mechanical-workshop-production

# Check cluster status
kubectl cluster-info

# Debug node issues
kubectl describe nodes
kubectl get events --sort-by='.lastTimestamp'
```

#### 4. Database Connection Issues

```bash
# Test database connectivity from a pod
kubectl run mysql-test --rm -it --image=mysql:8.0 -- mysql -h <db-endpoint> -u workshopuser -p
```

### Debug Commands

```bash
# Enable Terraform debug logging
export TF_LOG=DEBUG
terraform apply

# Check AWS resources
aws eks list-clusters
aws rds describe-db-instances
aws ec2 describe-vpcs
```

### Recovery Procedures

```bash
# Backup current state
cp terraform.tfstate terraform.tfstate.backup

# Force recreation of specific resource
terraform taint aws_instance.example
terraform apply
```

## 📚 Additional Resources

- [Terraform Best Practices](https://www.terraform-best-practices.com/)
- [AWS EKS Best Practices](https://aws.github.io/aws-eks-best-practices/)
- [Kubernetes Documentation](https://kubernetes.io/docs/)

---

**Importante**: Esta é uma configuração inicial simplificada. Para ambientes de produção críticos, considere adicionar:
- Monitoramento e alertas
- Backup e disaster recovery
- Múltiplos ambientes (staging/prod)
- SSL/TLS e domínio personalizado
- Pipeline de CI/CD
