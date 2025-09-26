#!/bin/bash
# terraform/scripts/deploy-local.sh

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
LOCALSTACK_ENDPOINT="http://localhost:4566"
TERRAFORM_DIR="$(dirname "$0")/../environments/local"
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

check_prerequisites() {
    log_info "Checking prerequisites..."

    # Check if LocalStack is running
    if ! curl -s "$LOCALSTACK_ENDPOINT/health" > /dev/null; then
        log_error "LocalStack is not running. Please start LocalStack first."
        log_info "To start LocalStack, run: localstack start"
        exit 1
    fi

    # Check if Terraform is installed
    if ! command -v terraform &> /dev/null; then
        log_error "Terraform is not installed. Please install Terraform first."
        exit 1
    fi

    # Check if kubectl is installed
    if ! command -v kubectl &> /dev/null; then
        log_error "kubectl is not installed. Please install kubectl first."
        exit 1
    fi

    # Check if Docker is running
    if ! docker info > /dev/null 2>&1; then
        log_error "Docker is not running. Please start Docker first."
        exit 1
    fi

    log_success "All prerequisites are met!"
}

start_localstack() {
    log_info "Starting LocalStack services..."

    # Create LocalStack services
    cat > /tmp/localstack-services.json << EOF
{
    "services": ["ec2", "eks", "rds", "iam", "sts", "elasticloadbalancing", "autoscaling", "kms", "secretsmanager"]
}
EOF

    # Wait for LocalStack to be fully ready
    max_attempts=30
    attempt=0
    while [ $attempt -lt $max_attempts ]; do
        if curl -s "$LOCALSTACK_ENDPOINT/_localstack/health" | grep -q '"eks": "available"'; then
            log_success "LocalStack is ready!"
            break
        fi
        log_info "Waiting for LocalStack to be ready... (attempt $((attempt + 1))/$max_attempts)"
        sleep 10
        ((attempt++))
    done

    if [ $attempt -eq $max_attempts ]; then
        log_error "LocalStack failed to start properly after $((max_attempts * 10)) seconds"
        exit 1
    fi
}

deploy_terraform() {
    log_info "Deploying infrastructure with Terraform..."

    cd "$TERRAFORM_DIR"

    # Initialize Terraform
    log_info "Initializing Terraform..."
    terraform init

    # Plan
    log_info "Planning Terraform deployment..."
    terraform plan -out=tfplan

    # Apply
    log_info "Applying Terraform configuration..."
    terraform apply tfplan

    # Clean up plan file
    rm -f tfplan

    log_success "Infrastructure deployed successfully!"
}

configure_kubectl() {
    log_info "Configuring kubectl for LocalStack EKS..."

    cd "$TERRAFORM_DIR"

    # Get cluster info from Terraform output
    CLUSTER_NAME=$(terraform output -raw cluster_id 2>/dev/null || echo "${PROJECT_NAME}-${ENVIRONMENT}")
    CLUSTER_ENDPOINT=$(terraform output -raw cluster_endpoint 2>/dev/null || echo "$LOCALSTACK_ENDPOINT")

    # Configure kubectl for LocalStack
    kubectl config set-cluster "$CLUSTER_NAME" \
        --server="$CLUSTER_ENDPOINT" \
        --insecure-skip-tls-verify=true

    kubectl config set-credentials "localstack" \
        --token="test-token"

    kubectl config set-context "$CLUSTER_NAME" \
        --cluster="$CLUSTER_NAME" \
        --user="localstack"

    kubectl config use-context "$CLUSTER_NAME"

    log_success "kubectl configured for LocalStack!"
}

deploy_k8s_manifests() {
    log_info "Deploying Kubernetes manifests..."

    # Navigate to k8s directory
    K8S_DIR="$(dirname "$0")/../../k8s"

    if [ ! -d "$K8S_DIR" ]; then
        log_error "Kubernetes directory not found: $K8S_DIR"
        exit 1
    fi

    cd "$K8S_DIR"

    # Apply development overlay
    log_info "Applying Kubernetes development configuration..."
    kubectl apply -k overlays/development/

    # Wait for pods to be ready
    log_info "Waiting for pods to be ready..."
    kubectl wait --for=condition=ready pod -l app.kubernetes.io/name=mysql -n smart-mechanical-workshop-dev --timeout=300s || true
    kubectl wait --for=condition=ready pod -l app.kubernetes.io/name=smart-mechanical-workshop-api -n smart-mechanical-workshop-dev --timeout=300s || true

    log_success "Kubernetes manifests deployed successfully!"
}

show_access_info() {
    log_info "Getting service access information..."

    # Get service information
    echo ""
    echo "=================================================="
    echo "🌐 LOCAL DEVELOPMENT ENVIRONMENT ACCESS INFO"
    echo "=================================================="

    # Show kubectl context
    echo "📋 Kubectl Context: $(kubectl config current-context)"

    # Show services
    echo ""
    echo "🔗 Services:"
    kubectl get svc -n smart-mechanical-workshop-dev 2>/dev/null || log_warning "Could not get services"

    # Show pods
    echo ""
    echo "📦 Pods:"
    kubectl get pods -n smart-mechanical-workshop-dev 2>/dev/null || log_warning "Could not get pods"

    # Show access URLs
    echo ""
    echo "🌐 Access URLs (after port-forwarding):"
    echo "  API: http://localhost:5180"
    echo "  MailHog: http://localhost:8025"
    echo "  MySQL: localhost:3306"

    echo ""
    echo "💡 To access services, run:"
    echo "  kubectl port-forward svc/dev-api-service 5180:5180 -n smart-mechanical-workshop-dev"
    echo "  kubectl port-forward svc/dev-mailhog-service 8025:8025 -n smart-mechanical-workshop-dev"
    echo "  kubectl port-forward svc/dev-mysql-service 3306:3306 -n smart-mechanical-workshop-dev"

    echo ""
    echo "=================================================="
}

cleanup_on_error() {
    log_error "Deployment failed. Cleaning up..."
    cd "$TERRAFORM_DIR" 2>/dev/null || true
    terraform destroy -auto-approve 2>/dev/null || true
}

main() {
    log_info "Starting LocalStack deployment for $PROJECT_NAME..."

    # Set trap for cleanup on error
    trap cleanup_on_error ERR

    # Run deployment steps
    check_prerequisites
    start_localstack
    deploy_terraform
    configure_kubectl
    deploy_k8s_manifests
    show_access_info

    log_success "Local deployment completed successfully! 🎉"
    log_info "Your development environment is ready to use."
}

# Run main function
main "$@"
