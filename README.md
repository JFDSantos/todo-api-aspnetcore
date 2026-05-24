# ToDo App — Full Stack

Aplicação completa de gerenciamento de tarefas com **Angular 21** no frontend e **ASP.NET Core 8** no backend, seguindo princípios **SOLID** e arquitetura em camadas.

---

## Sobre o Projeto

Interface web moderna integrada a uma API REST robusta para criar, listar, atualizar e deletar tarefas. Implementa transições de estado semânticas (State Machine Pattern), validações em múltiplas camadas e um frontend reativo com Angular Signals.

**Status:** Implementação completa

---

## Tecnologias

### Backend
- **ASP.NET Core 8.0** — Framework web
- **Entity Framework Core 8** — ORM / data access
- **SQL Server** — Banco de dados relacional
- **FluentValidation 11** — Validação de dados
- **AutoMapper 13** — Mapeamento de objetos
- **Swagger / OpenAPI** — Documentação de endpoints

### Frontend
- **Angular 21** — Framework SPA
- **Angular Signals** — Reatividade sem zone.js
- **TypeScript** — Tipagem estática
- **CSS (tokens + BEM)** — Estilização por componente

---

## Pré-requisitos

- **Node.js 20+** — [Download](https://nodejs.org)
- **.NET 8 SDK** — [Download](https://dotnet.microsoft.com/download)
- **SQL Server 2019+** — Instância local (`localhost\SQLEXPRESS`)

---

## Início Rápido

### 1. Clonar o repositório
```bash
git clone https://github.com/seu-usuario/todo-app.git
cd ToDo
```

### 2. Instalar dependências do frontend
```bash
cd frontend
npm install
cd ..
```

### 3. Configurar a Connection String

Edite `src/ToDo.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ToDoDb;Encrypt=false;TrustServerCertificate=true"
  }
}
```

### 4. Subir o projeto completo
```powershell
.\start.ps1
```

Isso abre duas janelas separadas:

| Serviço | URL |
|---------|-----|
| **Frontend** | http://localhost:4200 |
| **Backend** | http://localhost:5273 |

> O banco de dados e as tabelas são criados automaticamente na primeira execução (auto-migration).

---

## Endpoints da API

Base URL: `http://localhost:5273/api`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/tasks` | Listar todas as tarefas |
| GET | `/tasks/{id}` | Obter tarefa por ID |
| POST | `/tasks` | Criar nova tarefa |
| PUT | `/tasks/{id}` | Atualizar tarefa |
| PUT | `/tasks/{id}/start` | Iniciar tarefa |
| PUT | `/tasks/{id}/complete` | Concluir tarefa |
| PUT | `/tasks/{id}/reopen` | Reabrir tarefa |
| DELETE | `/tasks/{id}` | Deletar tarefa |

### Exemplo — Criar tarefa
```http
POST /api/tasks
Content-Type: application/json

{
  "title": "Minha tarefa",
  "description": "Descrição opcional"
}
```

### Códigos de Status

| Código | Significado |
|--------|------------|
| 200 | OK |
| 201 | Created |
| 204 | No Content (deletado) |
| 400 | Bad Request (validação) |
| 404 | Not Found |
| 500 | Internal Server Error |

### Swagger
Com a API rodando, acesse: **http://localhost:5273/swagger**

---

## Estado das Tarefas

```
Pendente (0) --start--> Em Progresso (1)
     ^                        |
     +-------reopen------  Concluída (2)
                                ^
                            complete
```

---

## Arquitetura

```
ToDo/
├── frontend/                      ← Angular 21 (SPA)
│   └── src/app/
│       ├── components/            (task-list, task-form)
│       ├── services/              (task.service.ts)
│       └── models/                (task.model.ts)
│
├── src/
│   ├── ToDo.Domain/               ← Entidades, Enums, Interfaces
│   ├── ToDo.Application/          ← DTOs, Services, Validators, AutoMapper
│   ├── ToDo.Infrastructure/       ← EF Core, Repositórios, Migrations
│   └── ToDo.API/                  ← Controllers, Program.cs
│
└── start.ps1                      ← Sobe front + back com 1 comando
```

### Fluxo de Requisição

```
Browser → Angular Component → TaskService → HTTP → Controller → Service → Repository → SQL Server
                                                                    ↓
                                                              FluentValidation
                                                              AutoMapper
```

---

## Modelo de Dados

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | INT (PK) | Identificador único |
| `Title` | VARCHAR(100) | Título (obrigatório) |
| `Description` | VARCHAR(MAX) | Descrição (opcional) |
| `CreatedAt` | DATETIME2 | Data de criação (auto-set) |
| `CompletedAt` | DATETIME2 | Data de conclusão (nullable) |
| `Status` | INT | 0=Pendente, 1=EmProgresso, 2=Concluída |

---

## Troubleshooting

**Erro de conexão com banco**
- Verifique se o SQL Server está rodando
- Confirme a connection string em `appsettings.json`

**Frontend não conecta ao backend**
- Certifique-se de que o backend está rodando em `http://localhost:5273`
- Verifique a configuração de CORS em `Program.cs`

**Build falha**
```bash
dotnet clean && dotnet restore && dotnet build
```

---

## Licença

MIT License
