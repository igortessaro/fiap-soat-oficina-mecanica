# Infraestrutura Terraform - Smart Mechanical Workshop

## 📋 Índice

- [Visão Geral da Arquitetura](#visão-geral-da-arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Início Rápido](#início-rápido)
- [Comandos](#comandos)
- [O que Será Criado](#o-que-será-criado)
- [Solução de Problemas](#solução-de-problemas)

## 🏗️ Visão Geral da Arquitetura

### Ambiente de Produção Único

```text
┌─────────────────────────────────────┐
│           AWS Production            │
├─────────────────────────────────────┤
│ • VPC com subnets públicas/privadas │
│ • EKS Cluster (Kubernetes)          │
│ • RDS MySQL                         │
│ • Security Groups                   │
│ • IAM Roles                         │
└─────────────────────────────────────┘
```

### Componentes

- **VPC**: Isolamento seguro de rede com subnets públicas e privadas
- **EKS**: Cluster Kubernetes gerenciado para orquestração de containers
- **RDS**: Banco de dados MySQL com backups automáticos e criptografia
- **Security Groups**: Regras de segurança de rede
- **IAM**: Roles e políticas para EKS e aplicações

## 📁 Estrutura de Diretórios

```text
terraform/
├── environments/
│   └── production/              # Ambiente AWS de produção
│       ├── main.tf
│       ├── variables.tf
│       ├── terraform.tfvars
│       └── versions.tf
├── modules/
│   ├── vpc/                     # VPC e networking
│   ├── rds/                     # Banco de dados RDS MySQL
│   ├── eks/                     # Cluster EKS e node groups
│   └── security/                # Segurança (IAM)
├── scripts/
│   └── utils.sh                # Funções utilitárias
├── Makefile                     # Comandos automatizados
└── README.md                    # Este arquivo
```

## 🔧 Pré-requisitos

### Software Necessário

| Ferramenta     | Versão   | Propósito                       |
| -------------- | -------- | ------------------------------- |
| **Terraform**  | >= 1.0   | Provisionamento de infraestrutura |
| **AWS CLI**    | >= 2.0   | Gerenciamento de recursos AWS     |
| **kubectl**    | >= 1.25  | Gerenciamento do Kubernetes       |
| **jq**         | latest   | Processamento JSON                |

### Comandos de Instalação

```bash
# macOS com Homebrew
brew install terraform awscli kubectl jq

# Ubuntu/Debian
curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -
sudo apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main"
sudo apt-get update && sudo apt-get install terraform
```

### Configuração AWS

```bash
# Configurar credenciais AWS
aws configure

# Verificar acesso
aws sts get-caller-identity
```

## 🚀 Início Rápido

### 1. Configurar Senha do Banco de Dados

```bash
# Gerar uma senha segura
export TF_VAR_db_password="$(openssl rand -base64 32)"

# Ou definir sua própria senha
export TF_VAR_db_password="sua-senha-segura"
```

### 2. Planejar Deploy (Simulação)

```bash
# Ver o que será criado
make plan
```

### 3. Deploy para Produção

```bash
# Deploy da infraestrutura
make deploy
```

### 4. Configurar kubectl

```bash
# Atualizar kubeconfig para acessar o cluster EKS
aws eks update-kubeconfig --name smart-mechanical-workshop-production
```

### 5. Verificar Deploy

```bash
# Verificar status do cluster
kubectl cluster-info

# Verificar nodes
kubectl get nodes

# Verificar status de produção
make status
```

## 📋 Comandos

Todos os comandos estão disponíveis através do Makefile:

```bash
make help      # Mostrar comandos disponíveis
make plan      # Planejar deploy (simulação)
make deploy    # Deploy para produção
make status    # Mostrar status atual
make output    # Mostrar saídas do Terraform
make destroy   # Destruir todos os recursos (CUIDADO!)
make format    # Formatar arquivos Terraform
make validate  # Validar configuração
make lint      # Verificar formatação do código
make clean     # Limpar arquivos temporários
```

## 🔧 O que Será Criado

### Infraestrutura de Rede

- **VPC** com CIDR 10.0.0.0/16
- **Subnets Públicas** em 2 zonas de disponibilidade (us-east-1a, us-east-1b)
- **Subnets Privadas** em 2 zonas de disponibilidade
- **Internet Gateway** para acesso à internet pública
- **NAT Gateways** para acesso à internet das subnets privadas
- **Tabelas de Rotas** e roteamento adequado

### Cluster EKS

- **Cluster EKS** versão 1.28
- **Node Group** com 1-3 instâncias t3.small (econômico)
- **Auto Scaling** habilitado
- **IAM Roles** para cluster e nodes
- **Security Groups** para comunicação segura

### Banco de Dados

- **RDS MySQL** instância db.t3.micro (menor disponível)
- **20GB** de armazenamento alocado (suficiente para testes)
- **Backups automatizados** desabilitados (economia)
- **Deploy Single AZ** (mais barato)
- **Posicionamento em subnet privada**
- **Security Groups** restringindo acesso

### Segurança

- **IAM Roles** com acesso de menor privilégio
- **Security Groups** com portas mínimas necessárias
- **Rede privada** para banco de dados e worker nodes
- **Armazenamento criptografado** para todos os componentes

## 🔧 Solução de Problemas

### Problemas Comuns

#### 1. Problemas de Autenticação AWS

```bash
# Verificar suas credenciais AWS
aws sts get-caller-identity

# Configurar perfil AWS se necessário
aws configure --profile workshop
export AWS_PROFILE=workshop
```

#### 2. Problemas de State do Terraform

```bash
# Se tiver problemas de travamento de state
terraform force-unlock LOCK_ID

# Se o state estiver corrompido
rm -rf .terraform terraform.tfstate*
terraform init
```

#### 3. Problemas de Acesso ao EKS

```bash
# Atualizar kubeconfig
aws eks update-kubeconfig --name smart-mechanical-workshop-production

# Verificar status do cluster
kubectl cluster-info

# Debug problemas de nodes
kubectl describe nodes
kubectl get events --sort-by='.lastTimestamp'
```

#### 4. Problemas de Conexão com o Banco

```bash
# Testar conectividade do banco a partir de um pod
kubectl run mysql-test --rm -it --image=mysql:8.0 -- mysql -h <db-endpoint> -u workshopuser -p
```

### Comandos de Debug

```bash
# Habilitar logs de debug do Terraform
export TF_LOG=DEBUG
terraform apply

# Verificar recursos AWS
aws eks list-clusters
aws rds describe-db-instances
aws ec2 describe-vpcs
```

### Procedimentos de Recuperação

```bash
# Backup do state atual
cp terraform.tfstate terraform.tfstate.backup

# Forçar recriação de recurso específico
terraform taint aws_instance.example
terraform apply
```

## 📚 Recursos Adicionais

- [Melhores Práticas do Terraform](https://www.terraform-best-practices.com/)
- [Melhores Práticas do AWS EKS](https://aws.github.io/aws-eks-best-practices/)
- [Documentação do Kubernetes](https://kubernetes.io/docs/)
