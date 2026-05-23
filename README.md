# ToDo API - ASP.NET Core

Uma API REST completa para gerenciamento de tarefas desenvolvida em **ASP.NET Core 8.0** seguindo **princípios SOLID** e **arquitetura em camadas**.

## 🎯 Visão Geral

API robusta que implementa operações CRUD completas para gerenciamento de tarefas com:
- ✅ Validação em camadas com FluentValidation
- ✅ Mapeamento automático com AutoMapper
- ✅ Entity Framework Core com SQL Server
- ✅ Injeção de dependência completa
- ✅ Logging estruturado
- ✅ Tratamento de erros HTTP apropriado
- ✅ Migrations automáticas ao iniciar

## 📋 Requisitos

- **.NET 8.0** ou superior
- **SQL Server** (Express, LocalDB ou outro)
- **Git**

## 🚀 Começando

### 1. Clonar o Repositório

```bash
git clone https://github.com/JFDSantos/todo-api-aspnetcore.git
cd todo-api-aspnetcore
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Configurar Banco de Dados

#### Opção A: SQL Server Local (Recomendado)

Edite `src/ToDo.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ToDoDb;Trusted_Connection=true;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True;"
  }
}
```

**Nota:** Se usar uma instância diferente, ajuste `Server=seu_servidor` e `Database=seu_banco`.

#### Opção B: Cadeia de Conexão Customizada

Edite `appsettings.json` com suas credenciais SQL Server:
```json
"Server=seu_servidor;Database=ToDoDb;User Id=sa;Password=sua_senha;Encrypt=False;"
```

### 4. Executar a API

```bash
cd src/ToDo.API
dotnet run
```

A API estará disponível em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`
- **Swagger:** `https://localhost:5001/swagger/index.html`

## 📚 Endpoints da API

### Listar Todas as Tarefas

```http
GET /api/tasks
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Estudar ASP.NET Core",
    "description": "Aprender princípios SOLID",
    "createdAt": "2026-05-23T10:30:00Z",
    "completedAt": null,
    "status": 0
  }
]
```

### Obter Tarefa por ID

```http
GET /api/tasks/{id}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Estudar ASP.NET Core",
  "description": "Aprender princípios SOLID",
  "createdAt": "2026-05-23T10:30:00Z",
  "completedAt": null,
  "status": 0
}
```

**Response (404 Not Found):**
```json
{
  "message": "Tarefa com ID 999 não encontrada"
}
```

### Criar Nova Tarefa

```http
POST /api/tasks
Content-Type: application/json

{
  "title": "Estudar ASP.NET Core",
  "description": "Aprender princípios SOLID"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Estudar ASP.NET Core",
  "description": "Aprender princípios SOLID",
  "createdAt": "2026-05-23T10:30:00Z",
  "completedAt": null,
  "status": 0
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Erro de validação",
  "errors": [
    {
      "PropertyName": "Title",
      "ErrorMessage": "Title must not be empty"
    }
  ]
}
```

### Atualizar Tarefa

```http
PUT /api/tasks/{id}
Content-Type: application/json

{
  "title": "Estudar ASP.NET Core Avançado",
  "description": "Aprender patterns e arquitetura",
  "status": 1,
  "completedAt": null
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Estudar ASP.NET Core Avançado",
  "description": "Aprender patterns e arquitetura",
  "createdAt": "2026-05-23T10:30:00Z",
  "completedAt": null,
  "status": 1
}
```

### Deletar Tarefa

```http
DELETE /api/tasks/{id}
```

**Response (204 No Content)** - Sem corpo de resposta

## 📊 Status da Tarefa

- `0` = Pendente
- `1` = Em Progresso
- `2` = Concluída

## 🏗️ Arquitetura

Projeto estruturado em **4 camadas**:

### 1. **Domain** (`src/ToDo.Domain`)
- Entidades: `TodoTask`
- Enums: `ETaskStatus`
- Interfaces de repositório: `ITaskRepository`
- **Sem dependências externas** (DIP - Dependency Inversion Principle)

### 2. **Application** (`src/ToDo.Application`)
- DTOs: `CreateTaskDto`, `UpdateTaskDto`, `TaskResponseDto`
- Validadores: `CreateTaskValidator`, `UpdateTaskValidator` (FluentValidation)
- Serviços: `ITaskService`, `TaskService`
- Mapeadores: `MappingProfile` (AutoMapper)
- **Depende de:** Domain

### 3. **Infrastructure** (`src/ToDo.Infrastructure`)
- DbContext: `ToDoDbContext` (Entity Framework Core)
- Repositório: `TaskRepository` (implementa `ITaskRepository`)
- Migrations: `InitialCreate`
- **Depende de:** Domain

### 4. **API** (`src/ToDo.API`)
- Controllers: `TasksController`
- Configuração: `Program.cs`
- Appsettings: `appsettings.json`
- **Depende de:** Application, Infrastructure, Domain

```
Domain (Entidades, Interfaces)
   ↑
   ├─ Application (Serviços, DTOs, Validação)
   ├─ Infrastructure (Repositórios, EF Core)
   └─ API (Controllers, DI)
```

## 🔧 Tecnologias Utilizadas

| Componente | Versão | Propósito |
|-----------|--------|----------|
| ASP.NET Core | 8.0 | Framework web |
| Entity Framework Core | 8.0.0 | ORM para SQL Server |
| FluentValidation | 11.9.0 | Validação em camadas |
| AutoMapper | 13.0.1 | Mapeamento de DTOs |
| SQL Server | - | Banco de dados |

## 🧪 Executar a Aplicação

### Desenvolvimento

```bash
cd src/ToDo.API
dotnet run
```

### Modo Release

```bash
cd src/ToDo.API
dotnet run --configuration Release
```

## 📝 Comandos Úteis

### Limpar Build

```bash
dotnet clean
```

### Restaurar Dependências

```bash
dotnet restore
```

### Compilar

```bash
dotnet build
```

### Publicar

```bash
dotnet publish -c Release -o ./publish
```

## 📦 Estrutura de Pastas

```
todo-api-aspnetcore/
├── src/
│   ├── ToDo.Domain/
│   │   ├── Entities/
│   │   │   └── TodoTask.cs
│   │   ├── Enums/
│   │   │   └── ETaskStatus.cs
│   │   └── Repositories/
│   │       └── ITaskRepository.cs
│   │
│   ├── ToDo.Application/
│   │   ├── Dtos/
│   │   │   ├── CreateTaskDto.cs
│   │   │   ├── UpdateTaskDto.cs
│   │   │   └── TaskResponseDto.cs
│   │   ├── Services/
│   │   │   ├── ITaskService.cs
│   │   │   └── TaskService.cs
│   │   ├── Validators/
│   │   │   ├── CreateTaskValidator.cs
│   │   │   └── UpdateTaskValidator.cs
│   │   └── Mappings/
│   │       └── MappingProfile.cs
│   │
│   ├── ToDo.Infrastructure/
│   │   ├── Data/
│   │   │   └── ToDoDbContext.cs
│   │   ├── Repositories/
│   │   │   └── TaskRepository.cs
│   │   └── Migrations/
│   │
│   └── ToDo.API/
│       ├── Controllers/
│       │   └── TasksController.cs
│       ├── Program.cs
│       ├── appsettings.json
│       └── Properties/
│
├── tests/
│   └── ToDo.Application.Tests/
│
└── README.md
```

## ✨ Princípios SOLID Aplicados

### **S**ingle Responsibility Principle
- Cada classe tem uma única responsabilidade
- `TaskService` = Lógica de negócio
- `TaskRepository` = Acesso a dados
- `TasksController` = Orquestração HTTP

### **O**pen/Closed Principle
- Aberto para extensão (novos validadores, novos mappers)
- Fechado para modificação (interfaces bem definidas)

### **L**iskov Substitution Principle
- `TaskRepository` substitui `ITaskRepository` sem quebrar o código
- DTOs são intercambiáveis nas camadas

### **I**nterface Segregation Principle
- `ITaskService` expõe apenas métodos necessários
- `ITaskRepository` agrupa operações de dados relacionadas

### **D**ependency Inversion Principle
- Dependências injetadas via Construtor (DI Container)
- Classes dependem de abstrações (interfaces), não implementações

## 🔐 Segurança

**Nota:** Esta é uma aplicação de demonstração. Para ambiente de produção:
- ✅ Implementar autenticação (JWT)
- ✅ Implementar autorização (Roles/Policies)
- ✅ Usar HTTPS obrigatório
- ✅ Validar entrada agressivamente
- ✅ Implementar rate limiting
- ✅ Usar secrets manager para credenciais

## 🐛 Troubleshooting

### Erro: "SQL Server does not exist"

**Solução:** Verifique se SQL Server está rodando e atualize `appsettings.json` com o servidor correto.

```bash
# Para SQL Express (padrão)
Server=localhost\SQLEXPRESS

# Para LocalDB
Server=(localdb)\mssqllocaldb
```

### Erro: "Connection timeout"

**Solução:** Verifique as credenciais e se SQL Server aceita conexões.

```bash
# Teste a conexão via sqlcmd (se instalado)
sqlcmd -S localhost\SQLEXPRESS -E
```

### Erro de Migração

**Solução:** Delete banco de dados manualmente (SQL Server Management Studio) e re-execute a API. Migrations serão criadas automaticamente.

## 📄 Licença

Este projeto é fornecido como-está para fins educacionais e de portfólio.

## 👤 Autor

**Jeferson Ferreira Santos**

- GitHub: [@JFDSantos](https://github.com/JFDSantos)
- Repositório: [todo-api-aspnetcore](https://github.com/JFDSantos/todo-api-aspnetcore)

## 📞 Suporte

Para dúvidas ou sugestões, abra uma **issue** no repositório do GitHub.

---

**Desenvolvido com ❤️ usando ASP.NET Core 8.0 e princípios SOLID**
