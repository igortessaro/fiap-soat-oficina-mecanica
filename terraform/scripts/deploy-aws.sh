#!/bin/bash
# terraform/scripts/deploy-aws.sh

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_NAME="smart-mechanical-workshop"
TERRAFORM_DIR="$(dirname "$0")/../environments"
K8S_DIR="$(dirname "$0")/../../k8s"

# Default values
ENVIRONMENT="staging"
AWS_REGION="us-east-1"
DRY_RUN=false
SKIP_K8S=false

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

usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -e, --environment ENVIRONMENT    Target environment (staging|production) [default: staging]"
    echo "  -r, --region REGION             AWS region [default: us-east-1]"
    echo "  -d, --dry-run                   Show what would be deployed without applying"
    echo "  -s, --skip-k8s                  Skip Kubernetes manifest deployment"
    echo "  -h, --help                      Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0 --environment staging"
    echo "  $0 --environment production --region us-west-2"
    echo "  $0 --dry-run --environment production"
    exit 1
}

parse_args() {
    while [[ $# -gt 0 ]]; do
        case $1 in
            -e|--environment)
                ENVIRONMENT="$2"
                shift 2
                ;;
            -r|--region)
                AWS_REGION="$2"
                shift 2
                ;;
            -d|--dry-run)
                DRY_RUN=true
                shift
                ;;
            -s|--skip-k8s)
                SKIP_K8S=true
                shift
                ;;
            -h|--help)
                usage
                ;;
            *)
                log_error "Unknown option: $1"
                usage
                ;;
        esac
    done

    # Validate environment
    if [[ ! "$ENVIRONMENT" =~ ^(staging|production)$ ]]; then
        log_error "Invalid environment: $ENVIRONMENT. Must be 'staging' or 'production'"
        exit 1
    fi
}

check_prerequisites() {
    log_info "Checking prerequisites for $ENVIRONMENT deployment..."

    # Check AWS CLI
    if ! command -v aws &> /dev/null; then
        log_error "AWS CLI is not installed. Please install AWS CLI first."
        exit 1
    fi

    # Check AWS credentials
    if ! aws sts get-caller-identity > /dev/null 2>&1; then
        log_error "AWS credentials not configured. Please run 'aws configure' first."
        exit 1
    fi

    # Check Terraform
    if ! command -v terraform &> /dev/null; then
        log_error "Terraform is not installed. Please install Terraform first."
        exit 1
    fi

    # Check kubectl
    if ! command -v kubectl &> /dev/null; then
        log_error "kubectl is not installed. Please install kubectl first."
        exit 1
    fi

    # Check helm
    if ! command -v helm &> /dev/null; then
        log_error "Helm is not installed. Please install Helm first."
        exit 1
    fi

    # Check required environment variables for production
    if [ "$ENVIRONMENT" = "production" ]; then
        if [ -z "$TF_VAR_db_password" ]; then
            log_error "TF_VAR_db_password environment variable is required for production"
            exit 1
        fi

        if [ -z "$TF_VAR_domain_name" ]; then
            log_error "TF_VAR_domain_name environment variable is required for production"
            exit 1
        fi

        if [ -z "$TF_VAR_ssl_certificate_arn" ]; then
            log_error "TF_VAR_ssl_certificate_arn environment variable is required for production"
            exit 1
        fi
    fi

    log_success "All prerequisites are met!"
}

setup_backend() {
    log_info "Setting up Terraform backend for $ENVIRONMENT..."

    local bucket_name="${PROJECT_NAME}-terraform-state-${ENVIRONMENT}"
    local dynamodb_table="${PROJECT_NAME}-terraform-locks-${ENVIRONMENT}"

    # Create S3 bucket for state if it doesn't exist
    if ! aws s3 ls "s3://$bucket_name" > /dev/null 2>&1; then
        log_info "Creating S3 bucket for Terraform state: $bucket_name"
        aws s3 mb "s3://$bucket_name" --region "$AWS_REGION"

        # Enable versioning
        aws s3api put-bucket-versioning \
            --bucket "$bucket_name" \
            --versioning-configuration Status=Enabled

        # Enable encryption
        aws s3api put-bucket-encryption \
            --bucket "$bucket_name" \
            --server-side-encryption-configuration '{
                "Rules": [
                    {
                        "ApplyServerSideEncryptionByDefault": {
                            "SSEAlgorithm": "AES256"
                        }
                    }
                ]
            }'
    fi

    # Create DynamoDB table for locking if it doesn't exist
    if ! aws dynamodb describe-table --table-name "$dynamodb_table" > /dev/null 2>&1; then
        log_info "Creating DynamoDB table for Terraform locking: $dynamodb_table"
        aws dynamodb create-table \
            --table-name "$dynamodb_table" \
            --attribute-definitions AttributeName=LockID,AttributeType=S \
            --key-schema AttributeName=LockID,KeyType=HASH \
            --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 \
            --region "$AWS_REGION"

        # Wait for table to be active
        aws dynamodb wait table-exists --table-name "$dynamodb_table" --region "$AWS_REGION"
    fi

    log_success "Backend setup completed!"
}

deploy_terraform() {
    log_info "Deploying infrastructure with Terraform..."

    local env_dir="$TERRAFORM_DIR/$ENVIRONMENT"

    if [ ! -d "$env_dir" ]; then
        log_error "Environment directory not found: $env_dir"
        exit 1
    fi

    cd "$env_dir"

    # Initialize Terraform
    log_info "Initializing Terraform..."
    terraform init

    # Plan
    log_info "Planning Terraform deployment..."
    if [ "$DRY_RUN" = true ]; then
        terraform plan
        log_info "Dry run completed. No changes were applied."
        return 0
    else
        terraform plan -out=tfplan
    fi

    # Confirm before applying in production
    if [ "$ENVIRONMENT" = "production" ]; then
        echo ""
        log_warning "⚠️  You are about to deploy to PRODUCTION!"
        read -p "Are you sure you want to continue? (yes/no): " confirm
        if [ "$confirm" != "yes" ]; then
            log_info "Deployment cancelled by user."
            rm -f tfplan
            exit 0
        fi
    fi

    # Apply
    log_info "Applying Terraform configuration..."
    terraform apply tfplan

    # Clean up plan file
    rm -f tfplan

    log_success "Infrastructure deployed successfully!"
}

configure_kubectl() {
    log_info "Configuring kubectl for EKS cluster..."

    local env_dir="$TERRAFORM_DIR/$ENVIRONMENT"
    cd "$env_dir"

    # Get cluster name from Terraform output
    local cluster_name
    cluster_name=$(terraform output -raw cluster_id)

    # Update kubeconfig
    aws eks update-kubeconfig \
        --region "$AWS_REGION" \
        --name "$cluster_name"

    # Verify connection
    if kubectl cluster-info > /dev/null 2>&1; then
        log_success "kubectl configured successfully!"
    else
        log_error "Failed to configure kubectl"
        exit 1
    fi
}

deploy_k8s_manifests() {
    if [ "$SKIP_K8S" = true ]; then
        log_info "Skipping Kubernetes manifest deployment as requested."
        return 0
    fi

    log_info "Deploying Kubernetes manifests..."

    if [ ! -d "$K8S_DIR" ]; then
        log_error "Kubernetes directory not found: $K8S_DIR"
        exit 1
    fi

    cd "$K8S_DIR"

    # Apply environment-specific overlay
    log_info "Applying Kubernetes $ENVIRONMENT configuration..."
    kubectl apply -k "overlays/$ENVIRONMENT/"

    # Wait for deployments to be ready
    log_info "Waiting for deployments to be ready..."
    kubectl wait --for=condition=available deployment \
        -l app.kubernetes.io/part-of=smart-mechanical-workshop \
        -n "smart-mechanical-workshop-${ENVIRONMENT}" \
        --timeout=600s

    log_success "Kubernetes manifests deployed successfully!"
}

show_access_info() {
    log_info "Getting service access information..."

    local env_dir="$TERRAFORM_DIR/$ENVIRONMENT"
    cd "$env_dir"

    echo ""
    echo "=================================================="
    echo "🌐 $ENVIRONMENT ENVIRONMENT ACCESS INFO"
    echo "=================================================="

    # Show Terraform outputs
    echo "🏗️  Infrastructure:"
    terraform output

    # Show kubectl context
    echo ""
    echo "📋 Kubectl Context: $(kubectl config current-context)"

    # Show services
    echo ""
    echo "🔗 Services:"
    kubectl get svc -n "smart-mechanical-workshop-${ENVIRONMENT}" 2>/dev/null || log_warning "Could not get services"

    # Show ingress (for production)
    if [ "$ENVIRONMENT" = "production" ]; then
        echo ""
        echo "🌐 Ingress:"
        kubectl get ingress -n "smart-mechanical-workshop-production" 2>/dev/null || log_warning "Could not get ingress"
    fi

    # Show pods
    echo ""
    echo "📦 Pods:"
    kubectl get pods -n "smart-mechanical-workshop-${ENVIRONMENT}" 2>/dev/null || log_warning "Could not get pods"

    echo ""
    echo "=================================================="
}

cleanup_on_error() {
    log_error "Deployment failed. Check the logs above for details."
    log_info "You may need to clean up resources manually."
}

main() {
    parse_args "$@"

    log_info "Starting AWS deployment for $PROJECT_NAME in $ENVIRONMENT environment..."

    # Set trap for cleanup on error
    trap cleanup_on_error ERR

    # Run deployment steps
    check_prerequisites
    setup_backend
    deploy_terraform

    if [ "$DRY_RUN" = false ]; then
        configure_kubectl
        deploy_k8s_manifests
        show_access_info

        log_success "$ENVIRONMENT deployment completed successfully! 🎉"
        log_info "Your $ENVIRONMENT environment is ready to use."
    fi
}

# Run main function
main "$@"
