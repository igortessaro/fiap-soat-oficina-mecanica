.PHONY: help deploy plan destroy clean lint format validate status output

# Default target
help: ## Show this help message
	@echo "Smart Mechanical Workshop - Terraform Commands"
	@echo ""
	@echo "Available commands:"
	@awk 'BEGIN {FS = ":.*##"; printf "\n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)

##@ Production Deployment
deploy: ## Deploy to production
	@echo "🚀 Deploying to production..."
	@cd environments/production && \
		terraform init && \
		terraform plan -out=tfplan && \
		terraform apply tfplan && \
		rm tfplan
	@echo "✅ Deployment completed!"

plan: ## Plan production deployment (dry-run)
	@echo "📋 Planning production deployment..."
	@cd environments/production && \
		terraform init && \
		terraform plan

destroy: ## Destroy production environment (CAREFUL!)
	@echo "⚠️  WARNING: This will destroy ALL production resources!"
	@read -p "Type 'yes' to confirm: " confirm && [ "$$confirm" = "yes" ]
	@cd environments/production && \
		terraform destroy

##@ Utilities
format: ## Format Terraform files
	@echo "🎨 Formatting Terraform files..."
	@terraform fmt -recursive .
	@echo "✅ All files formatted"

validate: ## Validate Terraform configuration
	@echo "🔍 Validating Terraform configuration..."
	@cd environments/production && \
		terraform init -backend=false && \
		terraform validate
	@echo "✅ Configuration is valid"

lint: ## Lint Terraform files
	@echo "🔍 Linting Terraform files..."
	@terraform fmt -check -recursive . || (echo "❌ Files need formatting. Run 'make format'" && exit 1)
	@echo "✅ All files are properly formatted"

clean: ## Clean up temporary files
	@echo "🧹 Cleaning up..."
	@find . -name ".terraform" -type d -exec rm -rf {} + 2>/dev/null || true
	@find . -name "*.tfplan" -delete 2>/dev/null || true
	@find . -name ".terraform.lock.hcl" -delete 2>/dev/null || true
	@echo "✅ Cleanup completed"

##@ Info
status: ## Show production status
	@echo "📊 Production Status:"
	@cd environments/production && terraform show 2>/dev/null || echo "No state found"

output: ## Show Terraform outputs
	@echo "📤 Production Outputs:"
	@cd environments/production && terraform output 2>/dev/null || echo "No outputs found"
