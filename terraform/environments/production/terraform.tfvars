aws_region           = "us-east-1"
database_name        = "workshopdb"
db_username          = "workshopuser"
# db_password should be set via environment variable or AWS Secrets Manager
db_allocated_storage = 100
domain_name          = "workshop.example.com"
ssl_certificate_arn  = "arn:aws:acm:us-east-1:123456789:certificate/xxx-xxx-xxx"
