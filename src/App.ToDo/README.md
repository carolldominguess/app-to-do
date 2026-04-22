# App.ToDo — API RESTful de Gestão de Tarefas

API RESTful desenvolvida com **.NET 8** utilizando **Clean Architecture**, **Entity Framework Core**, **Dapper**, padrão **Repository + Unit of Work**, **Chain of Responsibility** e **PredicateBuilder** para filtros dinâmicos.

---

## Arquitetura

```
App.ToDo/
├── App.ToDo.Domain          # Entidades, Enums, FluentValidation, PredicateBuilder, Interfaces, Filtros, Paginação
├── App.ToDo.Application     # UseCases, Handlers (Chain of Responsibility), Requests, DI
├── App.ToDo.Infra           # DbContext, Mappings (Fluent API), Repositories, UnitOfWork, Dapper, DI
├── App.ToDo.WebApi          # Controllers, Requests/Responses, Swagger, Program.cs
└── App.ToDo.UnitTests       # Builders, Testes (xUnit + FluentAssertions + Moq)
```

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (para execução com containers)
- [SQL Server](https://www.microsoft.com/sql-server) (local ou via Docker)

---

## Rodando com Docker (recomendado)

```bash
cd src/App.ToDo

docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

Isso sobe:
- **API** em `http://localhost:9001`
- **Swagger** em `http://localhost:9001/swagger`
- **SQL Server** em `localhost:1433` com usuário `sa` e senha `YourStrong@Passw0rd`

---

## Rodando localmente (sem Docker)

### 1. Configurar connection string

Edite `App.ToDo.WebApi/appsettings.Development.json` com sua string de conexão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AppToDoDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

### 2. Criar o banco de dados (Code First)

Abra um terminal na raiz da solution e execute:

```bash
dotnet ef migrations add InitialCreate --project App.ToDo.Infra --startup-project App.ToDo.WebApi
dotnet ef database update --project App.ToDo.Infra --startup-project App.ToDo.WebApi
```

### 3. Executar a API

```bash
dotnet run --project App.ToDo.WebApi
```

Acesse o Swagger em: `http://localhost:5178/swagger`

---

## Rodando os testes

```bash
dotnet test App.ToDo.UnitTests
```

---

## Endpoints disponíveis

| Método   | Rota                         | Descrição                                    |
|----------|------------------------------|----------------------------------------------|
| `GET`    | `/api/todotask`              | Lista todas as tarefas (paginado)            |
| `GET`    | `/api/todotask/{id}`         | Busca uma tarefa pelo Id                     |
| `GET`    | `/api/todotask/search`       | Busca com filtros: status, data, título      |
| `POST`   | `/api/todotask`              | Cria uma nova tarefa                         |
| `PUT`    | `/api/todotask/{id}`         | Atualiza uma tarefa existente                |
| `DELETE` | `/api/todotask/{id}`         | Remove uma tarefa                            |

### Filtros disponíveis em `GET /api/todotask/search`

| Parâmetro    | Tipo       | Descrição                              |
|--------------|------------|----------------------------------------|
| `title`      | `string`   | Filtra por título (contém)             |
| `status`     | `int`      | 1=Pendente, 2=Em andamento, 3=Concluído|
| `dueDateFrom`| `datetime` | Data de vencimento inicial (inclusive) |
| `dueDateTo`  | `datetime` | Data de vencimento final (inclusive)   |
| `page`       | `int`      | Número da página (padrão: 1)           |
| `pageSize`   | `int`      | Itens por página (padrão: 10, max: 100)|

---

## Decisões técnicas

- **Clean Architecture**: separação clara de responsabilidades entre camadas, com dependências apontando sempre para o domínio.
- **Chain of Responsibility**: cada UseCase monta uma cadeia de handlers (`Handler<T>`), onde cada handler executa sua responsabilidade (ex: validação → persistência) e interrompe a cadeia se detectar erro.
- **PredicateBuilder**: constrói predicados `Expression<Func<T, bool>>` de forma dinâmica e composicional com `AND`/`OR`, sem concatenação de strings SQL.
- **Dapper** no `GetAllPaged`: para leituras sem filtro, o Dapper oferece melhor performance por eliminar o overhead do change tracker do EF Core.
- **Unit of Work**: agrupa operações de escrita em uma única transação via `Commit()`, garantindo consistência.
- **Code First**: as tabelas são gerenciadas por migrations do EF Core. Execute manualmente após qualquer alteração no modelo.
