#!/bin/bash
# terraform/scripts/utils.sh

# Common utility functions for Terraform scripts

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

# Logging functions
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

# Check if command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Wait for resource to be ready
wait_for_resource() {
    local resource_type="$1"
    local resource_name="$2"
    local namespace="$3"
    local timeout="${4:-300}"

    log_info "Waiting for $resource_type/$resource_name to be ready..."

    kubectl wait --for=condition=ready "$resource_type/$resource_name" \
        -n "$namespace" --timeout="${timeout}s" || {
        log_warning "$resource_type/$resource_name is not ready after ${timeout}s"
        return 1
    }

    log_success "$resource_type/$resource_name is ready!"
}

# Get service external IP
get_service_external_ip() {
    local service_name="$1"
    local namespace="$2"
    local timeout="${3:-300}"

    log_info "Getting external IP for service $service_name..."

    local end_time=$((SECONDS + timeout))
    while [ $SECONDS -lt $end_time ]; do
        local external_ip
        external_ip=$(kubectl get svc "$service_name" -n "$namespace" \
            -o jsonpath='{.status.loadBalancer.ingress[0].ip}' 2>/dev/null || echo "")

        if [ -n "$external_ip" ] && [ "$external_ip" != "null" ]; then
            echo "$external_ip"
            return 0
        fi

        sleep 5
    done

    log_warning "Failed to get external IP for $service_name after ${timeout}s"
    return 1
}

# Port forward with automatic cleanup
port_forward() {
    local service_name="$1"
    local local_port="$2"
    local remote_port="$3"
    local namespace="$4"

    log_info "Setting up port forward: localhost:$local_port -> $service_name:$remote_port"

    # Kill existing port forwards on the same port
    pkill -f "kubectl.*port-forward.*$local_port:" 2>/dev/null || true

    # Start port forward in background
    kubectl port-forward "svc/$service_name" "$local_port:$remote_port" -n "$namespace" &
    local pf_pid=$!

    # Wait a moment to ensure port forward is established
    sleep 2

    if kill -0 $pf_pid 2>/dev/null; then
        log_success "Port forward established (PID: $pf_pid)"
        echo $pf_pid
    else
        log_error "Failed to establish port forward"
        return 1
    fi
}

# Export these functions
export -f log_info log_success log_warning log_error
export -f command_exists wait_for_resource get_service_external_ip port_forward
