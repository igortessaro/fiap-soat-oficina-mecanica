#!/bin/bash
# terraform/scripts/destroy-local.sh

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
TERRAFORM_DIR="$(dirname "$0")/../environments/local"
K8S_DIR="$(dirname "$0")/../../k8s"
PROJECT_NAME="smart-mechanical-workshop"
ENVIRONMENT="local"

# Functions
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

confirm_destruction() {
    echo ""
    log_warning "⚠️  You are about to DESTROY the local development environment!"
    log_warning "This will remove all resources including:"
    echo "  - Kubernetes deployments and services"
    echo "  - LocalStack infrastructure"
    echo "  - All data in databases"
    echo ""
    read -p "Are you sure you want to continue? (yes/no): " confirm

    if [ "$confirm" != "yes" ]; then
        log_info "Destruction cancelled by user."
        exit 0
    fi
}

destroy_k8s_resources() {
    log_info "Destroying Kubernetes resources..."

    if [ -d "$K8S_DIR" ]; then
        cd "$K8S_DIR"

        # Delete development overlay
        kubectl delete -k overlays/development/ --ignore-not-found=true || true

        # Force delete namespace if stuck
        kubectl delete namespace smart-mechanical-workshop-dev --force --grace-period=0 || true

        log_success "Kubernetes resources destroyed!"
    else
        log_warning "Kubernetes directory not found: $K8S_DIR"
    fi
}

destroy_terraform() {
    log_info "Destroying Terraform infrastructure..."

    if [ -d "$TERRAFORM_DIR" ]; then
        cd "$TERRAFORM_DIR"

        # Destroy infrastructure
        terraform destroy -auto-approve

        log_success "Terraform infrastructure destroyed!"
    else
        log_error "Terraform directory not found: $TERRAFORM_DIR"
        exit 1
    fi
}

cleanup_kubectl() {
    log_info "Cleaning up kubectl configuration..."

    # Remove LocalStack context
    kubectl config delete-context "${PROJECT_NAME}-${ENVIRONMENT}" 2>/dev/null || true
    kubectl config delete-cluster "${PROJECT_NAME}-${ENVIRONMENT}" 2>/dev/null || true
    kubectl config delete-user "localstack" 2>/dev/null || true

    log_success "kubectl configuration cleaned up!"
}

main() {
    log_info "Starting local environment destruction for $PROJECT_NAME..."

    confirm_destruction
    destroy_k8s_resources
    destroy_terraform
    cleanup_kubectl

    log_success "Local environment destroyed successfully! 🗑️"
    log_info "LocalStack is still running. Stop it manually if needed: localstack stop"
}

# Run main function
main "$@"
