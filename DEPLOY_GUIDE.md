# Deploy Guide - Smart Mechanical Workshop

## � Deploy Completo do Zero

### �📋 Pré-requisitos

#### Ferramentas Necessárias
- AWS CLI configurado com AWS Academy
- Terraform >= 1.0
- kubectl
- Docker (para build de imagens, se necessário)

#### Variáveis de Ambiente
```bash
export TF_VAR_db_password="workshop123"  # Senha do RDS
export AWS_DEFAULT_REGION="us-east-1"
```

### 🏗️ 1. Criar Infraestrutura AWS com Terraform

#### 1.1 Inicializar e Aplicar Terraform
```bash
# Navegar para diretório do Terraform
cd terraform/environments/production

# Inicializar Terraform
terraform init

# Verificar plano
terraform plan

# Aplicar infraestrutura (isso levará ~15-20 minutos)
terraform apply -auto-approve
```

#### 1.2 Configurar kubectl para o EKS
```bash
# Configurar kubectl para acessar o cluster EKS recém-criado
aws eks update-kubeconfig --name smart-mechanical-workshop-production --region us-east-1

# Verificar conectividade
kubectl cluster-info
kubectl get nodes

# Aguardar nodes ficarem prontos
kubectl wait --for=condition=Ready nodes --all --timeout=300s
```

## � Deploy Automático (Recomendado)

### Método 1: Script Automatizado
```bash
# Execute o script de deploy automático
./k8s/overlays/aws-production/deploy-aws.sh
```

### Método 2: Kustomize Manual
```bash
# 1. Obter endpoint do RDS
cd terraform/environments/production
RDS_ENDPOINT=$(terraform output -raw rds_endpoint)
cd ../../../

# 2. Atualizar configurações AWS
sed -i "s/RDS_ENDPOINT_PLACEHOLDER/$RDS_ENDPOINT/g" k8s/overlays/aws-production/configmap-aws.yaml

# 3. Aplicar configurações
kubectl create namespace smart-mechanical-workshop --dry-run=client -o yaml | kubectl apply -f -
kubectl apply -f k8s/overlays/aws-production/configmap-aws.yaml

# 4. Deploy das aplicações usando kustomize
kubectl apply -k k8s/overlays/aws-production/
```

## � Deploy Manual (Alternativo)

### 1. Configurar dados de conexão do banco
```bash
cd terraform/environments/production
RDS_ENDPOINT=$(terraform output -raw rds_endpoint)
kubectl create namespace smart-mechanical-workshop --dry-run=client -o yaml | kubectl apply -f -
```

### 2. Aplicar manifestos base com patches AWS
```bash
# MailHog
kubectl apply -f k8s/base/mailhog-deployment.yaml
kubectl apply -f k8s/overlays/aws-production/services-loadbalancer.yaml

# API com configuração AWS
kubectl apply -f k8s/base/api-deployment.yaml
kubectl apply -f k8s/overlays/aws-production/api-deployment-aws.yaml
```

## 🔍 3. Verificação e Monitoramento

### 3.1 Verificar deployments

```bash
kubectl get all -n smart-mechanical-workshop
kubectl get pods -n smart-mechanical-workshop
kubectl logs -f deployment/api-deployment -n smart-mechanical-workshop
```

### 3.2 Obter endpoints externos

```bash
kubectl get services -n smart-mechanical-workshop
```

### 3.3 Testar conectividade

```bash
# Testar API
API_IP=$(kubectl get service api-service -n smart-mechanical-workshop -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
curl http://$API_IP/health

# Testar MailHog
MAILHOG_IP=$(kubectl get service mailhog-service -n smart-mechanical-workshop -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "MailHog Web UI: http://$MAILHOG_IP:8025"
```

## 📊 4. Inicializar Banco de Dados

### 4.1 Conectar ao RDS e executar scripts

```bash
# Executar scripts de inicialização
# Os scripts 001_init_database e 010_fill_tables devem ser executados
```

## 🧹 5. Limpeza (quando necessário)

### Para remover aplicações

```bash
kubectl delete namespace smart-mechanical-workshop
```

### Para remover infraestrutura

```bash
cd terraform/environments/production && terraform destroy
```

## � Estrutura de Arquivos

```
k8s/
├── base/                     # Configurações base reutilizáveis
│   ├── api-deployment.yaml
│   ├── mailhog-deployment.yaml
│   ├── services.yaml
│   └── configmap.yaml
└── overlays/
    └── aws-production/       # Configurações específicas para AWS
        ├── kustomization.yaml
        ├── configmap-aws.yaml
        ├── services-loadbalancer.yaml
        ├── api-deployment-aws.yaml
        └── deploy-aws.sh     # Script automatizado
```

## 📝 Vantagens da Nova Estrutura

- ✅ **Organização**: Arquivos YAML organizados e reutilizáveis
- ✅ **Kustomize**: Uso de overlays para diferentes ambientes
- ✅ **Economia**: Recursos otimizados para custos mínimos
- ✅ **Automação**: Script de deploy automatizado
- ✅ **Manutenção**: Fácil de manter e versionar
