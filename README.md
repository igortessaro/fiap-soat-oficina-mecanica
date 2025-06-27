# fiap-soat-oficina-mecanicca

## Docker

Para rodar a aplicação usando o docker compose, você deve executar o comando baixo na pasta raiz do projeto:

````bash
docker compose -p "smart-mechanical-workshop" up --build -d
````

### Database Migrations

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef migrations add AddClientTable --project src/Fiap.Soat.SmartMechanicalWorkshop.Infrastructure --startup-project src/Fiap.Soat.SmartMechanicalWorkshop.Api
```
