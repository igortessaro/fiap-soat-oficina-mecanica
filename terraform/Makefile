# terraform/Makefile

.PHONY: help local-deploy local-destroy aws-deploy-staging aws-deploy-production clean lint

# Default target
help: ## Show this help message
    @echo "Smart Mechanical Workshop - Terraform Commands"
    @echo ""
    @echo "Available commands:"
    @awk 'BEGIN {FS = ":.*##"; printf "\n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)

##@ Local Development (LocalStack)
local-deploy: ## Deploy to local LocalStack environment
    @chmod +x scripts/deploy-local.sh
    @scripts/deploy-local.sh

local-destroy: ## Destroy local LocalStack environment
    @chmod +x scripts/destroy-local.sh
    @scripts/destroy-local.sh

local-status: ## Show status of local environment
    @echo "🔍 Local Environment Status:"
    @kubectl get all -n smart-mechanical-workshop-dev 2>/dev/null || echo "No resources found"

##@ AWS Deployments
aws-deploy-staging: ## Deploy to AWS staging environment
    @chmod +x scripts/deploy-aws.sh
    @scripts/deploy-aws.sh --environment staging

aws-deploy-production: ## Deploy to AWS production environment
    @chmod +x scripts/deploy-aws.sh
    @scripts/deploy-aws.sh --environment production

aws-plan-staging: ## Plan AWS staging deployment (dry-run)
    @chmod +x scripts/deploy-aws.sh
    @scripts/deploy-aws.sh --environment staging --dry-run

aws-plan-production: ## Plan AWS production deployment (dry-run)
    @chmod +x scripts/deploy-aws.sh
    @scripts/deploy-aws.sh --environment production --dry-run

##@ Utilities
lint: ## Lint Terraform files
    @echo "🔍 Linting Terraform files..."
    @find . -name "*.tf" -exec terraform fmt -check {} \; || (echo "❌ Terraform files need formatting. Run 'make format'" && exit 1)
    @echo "✅ All Terraform files are properly formatted"

format: ## Format Terraform files
    @echo "🎨 Formatting Terraform files..."
    @find . -name "*.tf" -exec terraform fmt {} \;
    @echo "✅ All Terraform files formatted"

validate: ## Validate Terraform configurations
    @echo "🔍 Validating Terraform configurations..."
    @for dir in environments/*/; do \
        echo "Validating $$dir..."; \
        cd $$dir && terraform init -backend=false && terraform validate && cd ../..; \
    done
    @echo "✅ All Terraform configurations are valid"

clean: ## Clean up temporary files
    @echo "🧹 Cleaning up temporary files..."
    @find . -name ".terraform" -type d -exec rm -rf {} + 2>/dev/null || true
    @find . -name "*.tfplan" -delete 2>/dev/null || true
    @find . -name ".terraform.lock.hcl" -delete 2>/dev/null || true
    @echo "✅ Cleanup completed"

##@ Documentation
docs: ## Generate Terraform documentation
    @echo "📚 Generating Terraform documentation..."
    @command -v terraform-docs >/dev/null 2>&1 || (echo "❌ terraform-docs is not installed" && exit 1)
    @for dir in modules/*/; do \
        echo "Generating docs for $$dir..."; \
        terraform-docs markdown table $$dir > $$dir/README.md; \
    done
    @echo "✅ Documentation generated"

##@ Debug
debug-local: ## Debug local environment
    @echo "🐛 Local Environment Debug Info:"
    @echo "LocalStack Status:"
    @curl -s http://localhost:4566/health | jq . 2>/dev/null || echo "LocalStack not accessible"
    @echo ""
    @echo "Kubectl Context:"
    @kubectl config current-context 2>/dev/null || echo "No kubectl context"
    @echo ""
    @echo "Kubernetes Resources:"
    @kubectl get all -n smart-mechanical-workshop-dev 2>/dev/null || echo "No resources found"

debug-aws: ## Debug AWS environment
    @echo "🐛 AWS Environment Debug Info:"
    @echo "AWS Identity:"
    @aws sts get-caller-identity 2>/dev/null || echo "AWS credentials not configured"
    @echo ""
    @echo "Kubectl Context:"
    @kubectl config current-context 2>/dev/null || echo "No kubectl context"
    @echo ""
    @echo "EKS Clusters:"
    @aws eks list-clusters 2>/dev/null || echo "Cannot list EKS clusters"
