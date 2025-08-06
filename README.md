# fiap-soat-oficina-mecanicca

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=coverage)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=bugs)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=igortessaro_fiap-soat-oficina-mecanica&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=igortessaro_fiap-soat-oficina-mecanica)

## Docker

Para rodar a aplicação usando o docker compose, você deve executar o comando baixo na pasta raiz do projeto:

````bash
docker compose -p "smart-mechanical-workshop" up --build -d
````

### Database Migrations

Para gerenciar as migrations do Entity Framework Core, siga os passos abaixo:

#### 1. Instale a ferramenta dotnet-ef (caso ainda não tenha)

```bash
dotnet tool install --global dotnet-ef
```

#### 2. Crie uma nova migration

Execute o comando abaixo na raiz do projeto, substituindo `NOME_DA_MIGRATION` pelo nome desejado para a migration:

```bash
dotnet ef migrations add NOME_DA_MIGRATION --project src/Fiap.Soat.SmartMechanicalWorkshop.Infrastructure --startup-project src/Fiap.Soat.SmartMechanicalWorkshop.Api
```

- O parâmetro `--project` indica onde estão as classes de contexto e migrations.
- O parâmetro `--startup-project` indica onde está o projeto de inicialização (geralmente a API).

#### 3. Aplique as migrations no banco de dados

```bash
dotnet ef database update --project src/Fiap.Soat.SmartMechanicalWorkshop.Infrastructure --startup-project src/Fiap.Soat.SmartMechanicalWorkshop.Api
```

#### 4. Dicas

- Sempre confira se as entidades e configurações do seu DbContext estão corretas antes de criar uma migration.
- Caso altere ou adicione Value Objects (como Phone ou Address), lembre-se de configurar corretamente como tipos próprios (owned types) no método
  `OnModelCreating` do seu DbContext.
- Para desfazer a última migration (caso necessário, antes de aplicar no banco):

```bash
dotnet ef migrations remove --project src/Fiap.Soat.SmartMechanicalWorkshop.Infrastructure
```

Para mais informações, consulte

## Como rodar o MailHog

Rode o docker-compose e acesse [http://localhost:8025](http://localhost:8025), onde você poderá visualizar todos os e-mails enviados pela aplicação em
tempo real através de uma interface simples e intuitiva.

## Autenticação obrigatória para todos os endpoints

A partir de agora, todos os endpoints da aplicação — como /api/v1/auth/login e /api/v1/servicesorders/search — requerem autenticação para serem acessados.
Para isso, você deve gerar um token JWT através do endpoint de login, informando o e-mail de uma pessoa cadastrada com o perfil "employee". Após obter o
token, clique em "Authorize" no Swagger e insira o token conforme as instruções. Isso fará com que ele seja automaticamente incluído nos headers das
requisições protegidas.
Além disso, o endpoint de visualização de ordens de serviço por clientes exige dois parâmetros obrigatórios: o ID da ordem e o e-mail da pessoa cadastrada
ou vinculada àquela ordem.


## Rodar API local

```
# Start only database and mailhog for development
docker compose -f docker-compose.dev.yml -p "fiap-smart-mechanical-workshop-dev" up --build -d

# Stop the development environment
docker compose -f docker-compose.dev.yml -p "fiap-smart-mechanical-workshop-dev" down
```
```
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=workshopdb;user=workshopuser;password=workshop123;SslMode=none;AllowPublicKeyRetrieval=True;"
}
```


Com API

```
docker compose -f docker-compose.yml -p "fiap-smart-mechanical-workshop" up --build -d
```
