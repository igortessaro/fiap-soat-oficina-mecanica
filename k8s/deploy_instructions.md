# Tornar o script executável
chmod +x k8s/deploy.sh

# Deploy para development
./k8s/deploy.sh development
# ou
kubectl apply -k k8s/overlays/development/

# Deploy para staging
./k8s/deploy.sh staging
# ou
kubectl apply -k k8s/overlays/staging/

# Deploy para production
./k8s/deploy.sh production
# ou
kubectl apply -k k8s/overlays/production/

# Ver o que será aplicado (dry-run)
kubectl kustomize k8s/overlays/development/

# Deletar um ambiente
kubectl delete -k k8s/overlays/development/
