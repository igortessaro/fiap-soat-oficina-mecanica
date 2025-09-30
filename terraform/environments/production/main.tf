locals {
  environment  = "production"
  project_name = "smart-mechanical-workshop"

  vpc_cidr           = "10.0.0.0/16"
  availability_zones = ["us-east-1a", "us-east-1b"]  # Apenas 2 AZs para economizar

  db_instance_class   = "db.t3.micro"      # Menor instância disponível
  node_instance_types = ["t3.small"]       # Instâncias pequenas e econômicas
  node_desired_size   = 1                  # Apenas 1 node inicial
  node_min_size       = 1                  # Mínimo 1 node
  node_max_size       = 3                  # Máximo 3 nodes
}

# Data sources
data "aws_caller_identity" "current" {}

# VPC Module
module "vpc" {
  source = "../../modules/vpc"

  environment        = local.environment
  project_name       = local.project_name
  vpc_cidr           = local.vpc_cidr
  availability_zones = local.availability_zones
}

# RDS Module
module "rds" {
  source = "../../modules/rds"

  environment        = local.environment
  project_name       = local.project_name
  vpc_id             = module.vpc.vpc_id
  vpc_cidr           = module.vpc.vpc_cidr_block
  private_subnet_ids = module.vpc.private_subnet_ids

  database_name     = var.database_name
  master_username   = var.db_username
  master_password   = var.db_password
  instance_class    = local.db_instance_class
  allocated_storage = var.db_allocated_storage

  backup_retention_period = 0   # Sem backup
  multi_az                = false  # Single AZ
}

# EKS Module
module "eks" {
  source = "../../modules/eks"

  environment                  = local.environment
  project_name                 = local.project_name
  vpc_id                       = module.vpc.vpc_id
  private_subnet_ids           = module.vpc.private_subnet_ids
  public_subnet_ids            = module.vpc.public_subnet_ids
  rds_client_security_group_id = module.rds.rds_client_security_group_id

  node_desired_size   = local.node_desired_size
  node_max_size       = local.node_max_size
  node_min_size       = local.node_min_size
  node_instance_types = local.node_instance_types
}
