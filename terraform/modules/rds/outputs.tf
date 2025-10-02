output "db_instance_endpoint" {
  description = "RDS instance endpoint"
  value       = aws_db_instance.main.endpoint
}

output "db_instance_port" {
  description = "RDS instance port"
  value       = aws_db_instance.main.port
}

output "db_instance_id" {
  description = "RDS instance ID"
  value       = aws_db_instance.main.id
}

output "rds_client_security_group_id" {
  description = "Security group ID for RDS clients"
  value       = aws_security_group.rds_client.id
}

output "database_name" {
  description = "Database name"
  value       = aws_db_instance.main.db_name
}
