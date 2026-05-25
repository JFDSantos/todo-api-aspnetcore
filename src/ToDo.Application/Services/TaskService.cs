using AutoMapper;
using FluentValidation;
using ToDo.Application.Dtos;
using ToDo.Application.Validators;
using ToDo.Domain.Entities;
using ToDo.Domain.Repositories;

namespace ToDo.Application.Services
{
    /// <summary>
    /// Implementação do serviço de tarefas
    /// Responsabilidades:
    ///   - Lógica de negócio (validações)
    ///   - Orquestração entre repositório e DTOs
    /// 
    /// Aplica: SRP (responsável apenas por lógica de aplicação)
    /// Validação: Todas as validações de negócio aqui (ID > 0, recurso existe, etc)
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<CreateTaskDto> _createValidator;
        private readonly IValidator<UpdateTaskDto> _updateValidator;
        private readonly IMapper _mapper;

        public TaskService(
            ITaskRepository repository,
            IValidator<CreateTaskDto> createValidator,
            IValidator<UpdateTaskDto> updateValidator,
            IMapper mapper)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync()
        {
            var tasks = await _repository.GetAllAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            // Validar DTO via FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Mapear DTO para Entidade
            var task = new TodoTask(dto.Title, dto.Description);

            // Persistir
            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            // Validar DTO via FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Obter tarefa existente
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            // Validar regra de negócio: CompletedAt não pode ser anterior a CreatedAt
            var effectiveCreatedAt = dto.CreatedAt ?? task.CreatedAt;
            if (dto.CompletedAt.HasValue && dto.CompletedAt < effectiveCreatedAt)
            {
                throw new ValidationException("A data de conclusão não pode ser anterior à data de criação.");
            }

            // Atualizar propriedades usando AutoMapper
            _mapper.Map(dto, task);

            // Persistir
            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> StartAsync(int id)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            if (task.Status != Domain.Enums.ETaskStatus.Pendente)
            {
                throw new ValidationException("A tarefa só pode ser iniciada se estiver no status Pendente.");
            }

            // Mudar status para EmProgresso (1)
            task.Status = Domain.Enums.ETaskStatus.EmProgresso;

            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> CompleteAsync(int id)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            // Mudar status para Concluída (2) e setar CompletedAt automaticamente
            task.Status = Domain.Enums.ETaskStatus.Concluida;
            task.CompletedAt = DateTime.Now;

            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> ReopenAsync(int id)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            // Mudar status para EmProgresso (1) e zerar CompletedAt
            task.Status = Domain.Enums.ETaskStatus.EmProgresso;
            task.CompletedAt = null;

            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task DeleteAsync(int id)
        {
            // Validação de negócio: ID deve ser válido
            if (id <= 0)
            {
                throw new ValidationException("ID deve ser maior que zero.");
            }

            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        private static TaskResponseDto MapToDto(TodoTask task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt,
                Status = task.Status
            };
        }
    }
}
