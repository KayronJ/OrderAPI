# Order API

API RESTful desenvolvida em **ASP.NET Core (.NET 8)** como parte do
processo seletivo para a vaga de **Analista de Sistemas .NET Core**.\
A aplicação gerencia **Pedidos** e suas **Ocorrências**, implementando
regras de negócio específicas, autenticação, testes e documentação com
Swagger. Utilizando Aggregates Roots e Rich Domains.

------------------------------------------------------------------------

## Descrição do Projeto

A aplicação permite:\
- Cadastro de **Pedidos**.\
- Registro de **Ocorrências** vinculadas a pedidos.\
- Regras de negócio que controlam o fluxo das ocorrências e o status do
pedido.\
- Autenticação e autorização via **JWT Token**.\
- Logs com **Serilog**.\
- Testes unitários cobrindo casos críticos.\
- Documentação automática com **Swagger/OpenAPI**.

------------------------------------------------------------------------

## Tecnologias Utilizadas

-   [.NET 8](https://dotnet.microsoft.com/en-us/download)\
-   **ASP.NET Core Web API**\
-   **Entity Framework Core**\
-   **Repository Pattern**\
-   **Domínio Rico (DDD Light)**\
-   **Injeção de Dependência (DI)**\
-   **Serilog** (para logs)\
-   **Swagger / Swashbuckle** (para documentação)\
-   **xUnit** (para testes unitários)\
-   **DataAnnotations** (para validações)
-   **SQLServer** (local para persistencias)

------------------------------------------------------------------------

## Estrutura do Projeto

    src/
     ├── OrderAPI.Domain/               
     │    ├── Entities/
     │    ├── Enums/
     │    └── Interfaces/
     │
     ├── OrderAPI.Application/    
     │    ├── DependencyInjection/
     │    ├── DTOs/
     │    ├── Interfaces/
     │    └── Services/
     │
     ├── OrderAPI.Infrastructure/        
     │    ├── Configurations/
     │    ├── DependencyInjection/
     │    ├── Persistence/
     │    └── Repositories/
     │
     ├── OrderAPI.Tests/               
     │    ├── Domain/
     │    └── Services/
     │
     └── OrderAPI.WebApi/                
     │    └── Controllers/             

------------------------------------------------------------------------

## 📌 Regras de Negócio Implementadas

-   Não é possível cadastrar 2 ocorrências do mesmo tipo em um intervalo
    de **10 minutos**.\
-   A **segunda ocorrência** para um pedido deve ser marcada como
    **finalizadora** (`IndFinalizadora = true`).\
-   Se a ocorrência finalizadora for `EntregueComSucesso`, o pedido é
    marcado como **entregue** (`IndEntregue = true`).\
-   Se for qualquer outro tipo, o pedido é marcado como **não entregue**
    (`IndEntregue = false`).\
-   Não é permitido cadastrar ou excluir ocorrências em pedidos já
    concluídos.

------------------------------------------------------------------------

## 📑 Entidades

**Pedido** - `IdPedido` (int)\
- `NumeroPedido` (int)\
- `HoraPedido` (DateTime)\
- `IndEntregue` (bool)\
- `Ocorrencias` (List`<Ocorrencia>`{=html})

**Ocorrência** - `IdOcorrencia` (int)\
- `TipoOcorrencia` (ETipoOcorrencia)\
- `HoraOcorrencia` (DateTime)\
- `IndFinalizadora` (bool)

**Enum ETipoOcorrencia** - `EmRotaDeEntrega`\
- `EntregueComSucesso`\
- `ClienteAusente`\
- `AvariaNoProduto`

------------------------------------------------------------------------

## 🚀 Como Executar o Projeto

### Pré-requisitos

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)\
-   [SQL
    Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
    ou outro banco configurado

### Passos

1.  Clone o repositório:

    ``` bash
    git clone 
    cd 
    ```

2.  Configure a **connection string** no `appsettings.json`.\

3.  Rode as migrations do Entity Framework:

    ``` bash
    Add-Migration InitialCreate
    Update-Migration
    ```

4.  Execute a API:

    ``` bash
    dotnet run --project src/WebApi
    ```

5.  Acesse o Swagger:

        http://localhost:5000/swagger

------------------------------------------------------------------------

## 🔑 Autenticação

-   A autenticação é baseada em **JWT (Bearer Token)**.\

-   Após login via `/api/auth/login`, copie o token retornado.\

-   No Swagger, clique em **Authorize** e insira:

        Bearer {seu_token}

------------------------------------------------------------------------

## Testes

Os testes foram escritos utilizando **xUnit**.\
Para rodar:

``` bash
dotnet test
```

------------------------------------------------------------------------

## 📊 Logs

-   Implementados com **Serilog**.\
-   Logs gravados em console e no banco na tabela `Logs`.

------------------------------------------------------------------------

## Critérios Atendidos

-   [x] API em .NET 8\
-   [x] Repository Pattern\
-   [x] Domínio Rico\
-   [x] Injeção de Dependência\
-   [x] Logs com Serilog\
-   [x] Tratamento de Erros (Try-catch)\
-   [x] Testes Unitários (xUnit)\
-   [x] Swagger/OpenAPI

------------------------------------------------------------------------

## Diferenciais Implementados

-   Validações com DataAnnotations

------------------------------------------------------------------------

## Autor

**SKayron Bragança**\
📧 kayron.juan123@email.com\
🔗 [LinkedIn](https://linkedin.com/in/kayron-braganca)
