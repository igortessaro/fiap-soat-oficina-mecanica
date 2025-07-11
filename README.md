# fiap-soat-oficina-mecanicca

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
- Caso altere ou adicione Value Objects (como Phone ou Address), lembre-se de configurar corretamente como tipos próprios (owned types) no método `OnModelCreating` do seu DbContext.
- Para desfazer a última migration (caso necessário, antes de aplicar no banco):

```bash
dotnet ef migrations remove --project src/Fiap.Soat.SmartMechanicalWorkshop.Infrastructure
```

Para mais informações, consulte

## Como rodar o MailHog
Rode o docker-compose e acesse [http://localhost:8025](http://localhost:8025), onde você poderá visualizar todos os e-mails enviados pela aplicação em tempo real através de uma interface simples e intuitiva.
