# terraform/environments/local/versions.tf
terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.23"
    }
  }
}

# Provider configuration for LocalStack
provider "aws" {
  region                      = "us-east-1"
  access_key                  = "test"
  secret_key                  = "test"
  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_requesting_account_id  = true

  endpoints {
    ec2                    = "http://localhost:4566"
    eks                    = "http://localhost:4566"
    rds                    = "http://localhost:4566"
    iam                    = "http://localhost:4566"
    sts                    = "http://localhost:4566"
    elasticloadbalancing   = "http://localhost:4566"
    autoscaling           = "http://localhost:4566"
    kms                   = "http://localhost:4566"
    secretsmanager        = "http://localhost:4566"
  }
}

# Data source for EKS cluster
data "aws_eks_cluster" "cluster" {
  name = module.eks.cluster_id
  depends_on = [module.eks]
}

data "aws_eks_cluster_auth" "cluster" {
  name = module.eks.cluster_id
  depends_on = [module.eks]
}

# Kubernetes provider
provider "kubernetes" {
  host                   = data.aws_eks_cluster.cluster.endpoint
  cluster_ca_certificate = base64decode(data.aws_eks_cluster.cluster.certificate_authority[0].data)
  token                  = data.aws_eks_cluster_auth.cluster.token
}
