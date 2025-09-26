variable "aws_region" {
  description = "AWS region"
  type        = string
  default     = "us-east-1"
}

variable "database_name" {
  description = "Database name"
  type        = string
  default     = "workshopdb"
}

variable "db_username" {
  description = "Database username"
  type        = string
  default     = "workshopuser"
}

variable "db_password" {
  description = "Database password"
  type        = string
  sensitive   = true
}

variable "db_allocated_storage" {
  description = "Database allocated storage in GB"
  type        = number
  default     = 100
}

variable "domain_name" {
  description = "Domain name for the application"
  type        = string
}

variable "ssl_certificate_arn" {
  description = "ARN of the SSL certificate"
  type        = string
}
