# 📋 ToDo REST API

Uma API robusta e bem arquitetada para gerenciamento de tarefas, desenvolvida com **ASP.NET Core 8.0**, seguindo princípios **SOLID** e padrões de design modernos.

---

## 🎯 Sobre o Projeto

API REST completa para criar, ler, atualizar e deletar tarefas. Implementa transições de estado semânticas (State Machine Pattern) e validações robustas em múltiplas camadas.

**Deadline:** 25 de maio de 2026  
**Status:** ✅ Implementação completa

---

## 🚀 Tecnologias

- **ASP.NET Core 8.0** - Framework web moderno
- **Entity Framework Core 8.0.0** - ORM para data access
- **SQL Server** - Banco de dados relacional
- **FluentValidation 11.9.0** - Validação fluente de dados
- **AutoMapper 13.0.1** - Mapeamento de objetos
- **Swagger/OpenAPI** - Documentação automática de endpoints

---

## 📋 Pré-requisitos

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download)
- **SQL Server 2019+** - Instale ou use instância local
  - Instância: `localhost\SQLEXPRESS` (padrão)
- **Git** - Para clonar o repositório

---

## 🔧 Instalação e Setup

### 1. Clonar o repositório
```bash
git clone https://github.com/seu-usuario/todo-api-aspnetcore.git
cd ToDo
```

### 2. Restaurar dependências
```bash
dotnet restore
```

### 3. Configurar a Connection String

Edite o arquivo `src/ToDo.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ToDoDb;Encrypt=false;TrustServerCertificate=true"
  }
}
```

**Valores importantes:**
- `Server`: Ajuste conforme sua instância SQL Server
- `Database`: Nome do banco (será criado automaticamente)
- `Encrypt`: `false` (para desenvolvimento local)

### 4. Build do projeto
```bash
dotnet build
```

### 5. Rodar a API
```bash
cd src/ToDo.API
dotnet run
```

A API iniciará em: **https://localhost:7183**

**Obs:** O banco de dados e tabelas serão criados automaticamente na primeira execução (auto-migration).

---

## 🌐 Endpoints da API

Base URL: `https://localhost:7183/api`

### 📖 Listar todas as tarefas
```http
GET /tasks
```
**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Implementar API",
    "description": "Criar endpoints REST",
    "createdAt": "2026-05-23T10:00:00Z",
    "completedAt": null,
    "status": 0
  }
]
```

---

### 🔍 Obter tarefa por ID
```http
GET /tasks/{id}
```
**Exemplo:**
```http
GET /tasks/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Implementar API",
  "description": "Criar endpoints REST",
  "createdAt": "2026-05-23T10:00:00Z",
  "completedAt": null,
  "status": 0
}
```

**Response (404 Not Found):**
```json
{
  "error": "Tarefa com ID 999 não encontrada."
}
```

---

### ➕ Criar nova tarefa
```http
POST /tasks
Content-Type: application/json

{
  "title": "Implementar API",
  "description": "Criar endpoints REST"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Implementar API",
  "description": "Criar endpoints REST",
  "createdAt": "2026-05-23T10:00:00Z",
  "completedAt": null,
  "status": 0
}
```

**Validações:**
- ❌ Title vazio → 400 Bad Request
- ❌ Title > 100 caracteres → 400 Bad Request

---

### ✏️ Atualizar tarefa
```http
PUT /tasks/{id}
Content-Type: application/json

{
  "title": "Implementar API (Atualizado)",
  "description": "Criar endpoints REST com validações",
  "status": 1,
  "completedAt": null
}
```

**Validações:**
- ❌ ID ≤ 0 → 400 Bad Request
- ❌ Tarefa não encontrada → 404 Not Found
- ❌ CompletedAt < CreatedAt → 400 Bad Request

---

### ▶️ Iniciar tarefa
Transiciona de **Pendente** para **Em Progresso**.

```http
PUT /tasks/{id}/start
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Implementar API",
  "description": "Criar endpoints REST",
  "createdAt": "2026-05-23T10:00:00Z",
  "completedAt": null,
  "status": 1
}
```

---

### ✅ Concluir tarefa
Transiciona para **Concluída** e seta `completedAt` automaticamente.

```http
PUT /tasks/{id}/complete
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Implementar API",
  "description": "Criar endpoints REST",
  "createdAt": "2026-05-23T10:00:00Z",
  "completedAt": "2026-05-23T11:30:00Z",
  "status": 2
}
```

---

### 🔄 Reabrir tarefa
Transiciona de volta para **Em Progresso** e limpa `completedAt`.

```http
PUT /tasks/{id}/reopen
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Implementar API",
  "description": "Criar endpoints REST",
  "createdAt": "2026-05-23T10:00:00Z",
  "completedAt": null,
  "status": 1
}
```

---

### 🗑️ Deletar tarefa
```http
DELETE /tasks/{id}
```

**Response (204 No Content)** - Sem corpo

---

## 📊 Códigos de Status HTTP

| Código | Significado |
|--------|------------|
| **200** | OK - Operação bem-sucedida |
| **201** | Created - Recurso criado com sucesso |
| **204** | No Content - Deletado com sucesso |
| **400** | Bad Request - Validação falhou |
| **404** | Not Found - Tarefa não encontrada |
| **500** | Internal Server Error - Erro no servidor |

---

## 🏗️ Arquitetura

A API segue uma **arquitetura em camadas** com **princípios SOLID**:

```
ToDo/
├── src/
│   ├── ToDo.Domain/              ← Domain (Entidades, Enums, Interfaces)
│   │   ├── Entities/
│   │   │   └── TodoTask.cs       (Entidade principal)
│   │   ├── Enums/
│   │   │   └── ETaskStatus.cs    (Status: Pendente, EmProgresso, Concluída)
│   │   └── Repositories/
│   │       └── ITaskRepository.cs (Interface de repositório)
│   │
│   ├── ToDo.Application/          ← Application (DTOs, Services, Validators)
│   │   ├── Dtos/
│   │   │   ├── CreateTaskDto.cs
│   │   │   ├── UpdateTaskDto.cs
│   │   │   └── TaskResponseDto.cs
│   │   ├── Validators/
│   │   │   ├── CreateTaskValidator.cs
│   │   │   └── UpdateTaskValidator.cs
│   │   ├── Services/
│   │   │   ├── ITaskService.cs
│   │   │   └── TaskService.cs
│   │   └── Mappings/
│   │       └── MappingProfile.cs  (AutoMapper)
│   │
│   ├── ToDo.Infrastructure/       ← Infrastructure (EF Core, Repository, DB)
│   │   ├── Data/
│   │   │   └── ToDoDbContext.cs
│   │   ├── Repositories/
│   │   │   └── TaskRepository.cs
│   │   └── Migrations/
│   │
│   └── ToDo.API/                  ← API (Controllers, Configuration)
│       ├── Controllers/
│       │   └── TasksController.cs (8 Endpoints)
│       ├── Program.cs             (DI, Middleware)
│       └── appsettings.json       (Config)
```

### 🔄 Fluxo de Requisição

```
Request HTTP
    ↓
Controller (TasksController)
    ↓ (passa DTO)
Service (TaskService) ← Lógica de negócio, Validações
    ↓ (passa Entidade)
Repository (TaskRepository) ← Acesso a dados
    ↓
Entity Framework Core + SQL Server
```

---

## 🎨 Padrões de Design

### **State Machine Pattern**
Transições de estado semânticas através de endpoints dedicados:

```
Pendente (0) ──start──> Em Progresso (1)
    ↑                          ↓
    └──────reopen────── Concluída (2)
                              │
                          complete
```

### **Dependency Injection (DI)**
Todos os serviços registrados no `Program.cs` para desacoplamento.

### **Repository Pattern**
Abstração de acesso a dados através de `ITaskRepository`.

### **AutoMapper**
Mapeamento automático entre DTOs e Entidades.

---

## 📝 Status dos Endpoints

| Método | Endpoint | Status | Descrição |
|--------|----------|--------|-----------|
| GET | `/tasks` | ✅ | Listar todas |
| GET | `/tasks/{id}` | ✅ | Obter por ID |
| POST | `/tasks` | ✅ | Criar nova |
| PUT | `/tasks/{id}` | ✅ | Atualizar |
| PUT | `/tasks/{id}/start` | ✅ | Iniciar |
| PUT | `/tasks/{id}/complete` | ✅ | Concluir |
| PUT | `/tasks/{id}/reopen` | ✅ | Reabrir |
| DELETE | `/tasks/{id}` | ✅ | Deletar |

---

## 🧪 Testando a API

### Com Swagger (UI)
1. Rode a API: `dotnet run`
2. Acesse: https://localhost:7183/swagger/index.html
3. Teste os endpoints diretamente na interface

### Com cURL
```bash
# Listar tarefas
curl -X GET https://localhost:7183/api/tasks

# Criar tarefa
curl -X POST https://localhost:7183/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Minha tarefa","description":"Descrição"}'
```

### Com Postman / Insomnia
Importe a URL base: `https://localhost:7183/api` e comece a testar!

---

## 🐛 Troubleshooting

### "Tarefa não encontrada"
- Verifique o ID da tarefa no banco de dados
- Certifique-se de que a tarefa foi criada

### "Erro de conexão com banco"
- Verifique se SQL Server está rodando
- Confirme a connection string em `appsettings.json`
- Cheque o nome da instância: `localhost\SQLEXPRESS`

### "Build falha"
- Execute: `dotnet clean && dotnet restore && dotnet build`
- Verifique se tem .NET 8 SDK instalado: `dotnet --version`

---

## 📚 Estrutura do Banco de Dados

### Tabela: Tasks

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| `Id` | INT (PK) | Identificador único |
| `Title` | VARCHAR(100) | Título (obrigatório) |
| `Description` | VARCHAR(MAX) | Descrição (opcional) |
| `CreatedAt` | DATETIME2 | Data de criação (auto-set) |
| `CompletedAt` | DATETIME2 | Data de conclusão (nullable) |
| `Status` | INT | 0=Pendente, 1=EmProgresso, 2=Concluída |

---

## 🚀 Deployment (Próximos Passos)

- [ ] Criar pipeline CI/CD (GitHub Actions)
- [ ] Dockerizar a aplicação
- [ ] Deploy em Azure App Service
- [ ] Configurar HTTPS com certificado válido

---

## 📄 Licença

MIT License

---

## 👨‍💻 Autor

Desenvolvido como parte de um desafio de desenvolvimento backend.

**GitHub:** https://github.com/seu-usuario/todo-api-aspnetcore

---

## 📞 Suporte

Para dúvidas ou problemas, abra uma [issue](https://github.com/seu-usuario/todo-api-aspnetcore/issues) no repositório.

---

**Última atualização:** 23 de maio de 2026
