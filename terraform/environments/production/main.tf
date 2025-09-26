locals {
  environment   = "production"
  project_name  = "smart-mechanical-workshop"

  # Production configurations
  vpc_cidr             = "10.0.0.0/16"
  availability_zones   = ["us-east-1a", "us-east-1b", "us-east-1c"]

  # Production-grade configurations
  db_instance_class    = "db.r6g.large"
  node_instance_types  = ["m5.large", "m5.xlarge"]
  node_desired_size    = 3
  node_min_size        = 3
  node_max_size        = 10
}

# Data sources
data "aws_caller_identity" "current" {}

# VPC Module
module "vpc" {
  source = "../../modules/vpc"

  environment        = local.environment
  project_name       = local.project_name
  vpc_cidr          = local.vpc_cidr
  availability_zones = local.availability_zones
}

# Security Module
module "security" {
  source = "../../modules/security"

  environment  = local.environment
  project_name = local.project_name
}

# RDS Module
module "rds" {
  source = "../../modules/rds"

  environment         = local.environment
  project_name        = local.project_name
  vpc_id              = module.vpc.vpc_id
  private_subnet_ids  = module.vpc.private_subnet_ids

  database_name       = var.database_name
  master_username     = var.db_username
  master_password     = var.db_password
  instance_class      = local.db_instance_class
  allocated_storage   = var.db_allocated_storage

  # Production settings
  backup_retention_period = 30
  multi_az               = true
}

# EKS Module
module "eks" {
  source = "../../modules/eks"

  environment                   = local.environment
  project_name                  = local.project_name
  vpc_id                       = module.vpc.vpc_id
  private_subnet_ids           = module.vpc.private_subnet_ids
  public_subnet_ids            = module.vpc.public_subnet_ids
  rds_client_security_group_id = module.rds.rds_client_security_group_id

  node_desired_size   = local.node_desired_size
  node_max_size       = local.node_max_size
  node_min_size       = local.node_min_size
  node_instance_types = local.node_instance_types
}

# AWS Load Balancer Controller
resource "helm_release" "aws_load_balancer_controller" {
  name       = "aws-load-balancer-controller"
  repository = "https://aws.github.io/eks-charts"
  chart      = "aws-load-balancer-controller"
  namespace  = "kube-system"

  set {
    name  = "clusterName"
    value = module.eks.cluster_id
  }

  set {
    name  = "serviceAccount.create"
    value = "true"
  }

  set {
    name  = "serviceAccount.name"
    value = "aws-load-balancer-controller"
  }

  depends_on = [module.eks]
}

# Cluster Autoscaler
resource "helm_release" "cluster_autoscaler" {
  name       = "cluster-autoscaler"
  repository = "https://kubernetes.github.io/autoscaler"
  chart      = "cluster-autoscaler"
  namespace  = "kube-system"

  set {
    name  = "autoDiscovery.clusterName"
    value = module.eks.cluster_id
  }

  set {
    name  = "awsRegion"
    value = var.aws_region
  }

  depends_on = [module.eks]
}

# Cert-Manager for SSL certificates
resource "helm_release" "cert_manager" {
  name             = "cert-manager"
  repository       = "https://charts.jetstack.io"
  chart            = "cert-manager"
  namespace        = "cert-manager"
  create_namespace = true

  set {
    name  = "installCRDs"
    value = "true"
  }

  depends_on = [module.eks]
}

# External Secrets Operator
resource "helm_release" "external_secrets" {
  name             = "external-secrets"
  repository       = "https://charts.external-secrets.io"
  chart            = "external-secrets"
  namespace        = "external-secrets-system"
  create_namespace = true

  depends_on = [module.eks]
}
