# Deploy Guide - Smart Mechanical Workshop

## 🚀 Deploy Automático com GitHub Actions (Recomendado)

Para facilitar o deploy, criamos workflows automatizados no GitHub Actions que fazem todo o processo para você.

### 📋 Setup Inicial dos Secrets

1. Vá para **Settings** → **Secrets and variables** → **Actions**
2. Configure os seguintes secrets:
   - `AWS_ACCESS_KEY_ID`: Sua Access Key da AWS
   - `AWS_SECRET_ACCESS_KEY`: Sua Secret Key da AWS
   - `AWS_SESSION_TOKEN`: Session Token (necessário para AWS Academy)

### 🎯 Como Fazer o Deploy Automático

1. Vá para **Actions** → **Complete Deployment Pipeline**
2. Clique em **Run workflow**
3. Configure:
   - Environment: `production`
   - Deploy Infrastructure: ✅
   - Deploy Applications: ✅
   - Database password: `workshop123`
4. Aguarde 15-20 minutos
5. Acesse as URLs fornecidas no resumo

> **💡 Dica:** O workflow automático resolve automaticamente todos os problemas de configuração de endpoints e dependências!

---

## 🛠️ Deploy Manual (Alternativo)

Se preferir fazer o deploy manualmente ou para fins de aprendizado:

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

## ❌ Limpeza dos Recursos

### Método 1: GitHub Actions (Automático)

1. Vá para **Actions** → **Destroy Infrastructure**
2. Clique em **Run workflow**
3. Aguarde a conclusão

### Método 2: Manual

```bash
cd terraform
terraform destroy
```

---

## 📚 Workflows Disponíveis no GitHub Actions

### 1. **Complete Deployment Pipeline** (`deploy-complete.yml`)
- Deploy completo de infraestrutura + aplicações
- Configuração automática de endpoints
- Inicialização do banco de dados
- Extração das URLs de acesso

### 2. **Deploy Infrastructure Only** (`deploy-infrastructure.yml`)
- Apenas criação da infraestrutura AWS (RDS + EKS)
- Útil para preparar ambiente

### 3. **Deploy Applications Only** (`deploy-applications.yml`)
- Apenas deploy das aplicações no Kubernetes
- Requer infraestrutura já criada

### 4. **Destroy Infrastructure** (`destroy-infrastructure.yml`)
- Remove todos os recursos AWS criados
- Limpeza completa do ambiente

### 📖 Documentação Completa
Para mais detalhes sobre os workflows, consulte: `.github/workflows/README.md`

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
