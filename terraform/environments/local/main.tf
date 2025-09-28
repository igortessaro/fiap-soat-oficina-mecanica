locals {
  environment   = "local"
  project_name  = "smart-mechanical-workshop"

  # LocalStack specific configurations
  vpc_cidr             = "10.0.0.0/16"
  availability_zones   = ["us-east-1a", "us-east-1b"]

  # Simplified configurations for local development
  db_instance_class    = "db.t3.micro"
  node_instance_types  = ["t3.small"]
  node_desired_size    = 1
  node_min_size        = 1
  node_max_size        = 2
}

# VPC Module
module "vpc" {
  source = "../../modules/vpc"

  environment        = local.environment
  project_name       = local.project_name
  vpc_cidr          = local.vpc_cidr
  availability_zones = local.availability_zones
}

# RDS Module
module "rds" {
  source = "../../modules/rds"

  environment         = local.environment
  project_name        = local.project_name
  vpc_id              = module.vpc.vpc_id
  private_subnet_ids  = module.vpc.private_subnet_ids

  database_name       = "workshopdb"
  master_username     = "workshopuser"
  master_password     = var.db_password
  instance_class      = local.db_instance_class
  allocated_storage   = 20

  # Local development settings
  backup_retention_period = 0
  multi_az               = false
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

# Kubernetes namespace
resource "kubernetes_namespace" "app" {
  metadata {
    name = "smart-mechanical-workshop-dev"
    labels = {
      environment = local.environment
      project     = local.project_name
    }
  }

  depends_on = [module.eks]
}

# Kubernetes Secret for Database Connection
resource "kubernetes_secret" "db_credentials" {
  metadata {
    name      = "smart-mechanical-workshop-secrets"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  data = {
    MYSQL_ROOT_PASSWORD = var.db_password
    MYSQL_PASSWORD      = var.db_password
  }

  type = "Opaque"

  depends_on = [kubernetes_namespace.app]
}

# Kubernetes ConfigMap
resource "kubernetes_config_map" "app_config" {
  metadata {
    name      = "smart-mechanical-workshop-config"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  data = {
    ASPNETCORE_ENVIRONMENT    = "Development"
    DB_HOST                  = split(":", module.rds.db_instance_endpoint)[0]
    DB_PORT                  = "3306"
    MYSQL_DATABASE           = module.rds.database_name
    MYSQL_USER               = "workshopuser"
    BASE_URL                 = "http://localhost:5180"
    SMTP_HOST                = "mailhog-service"
    SMTP_PORT                = "1025"
    DEFAULT_CONNECTION_STRING = "server=${split(":", module.rds.db_instance_endpoint)[0]};port=3306;database=${module.rds.database_name};user=workshopuser;password=${var.db_password};SslMode=none;AllowPublicKeyRetrieval=True;"
  }

  depends_on = [kubernetes_namespace.app]
}
