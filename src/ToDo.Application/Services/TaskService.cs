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
    ///   - Lógica de negócio (validações, mapeamentos)
    ///   - Orquestração entre repositório e DTOs
    /// 
    /// Aplica: SRP (responsável apenas por lógica de aplicação)
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<CreateTaskDto> _createValidator;
        private readonly IValidator<UpdateTaskDto> _updateValidator;

        public TaskService(
            ITaskRepository repository,
            IValidator<CreateTaskDto> createValidator,
            IValidator<UpdateTaskDto> updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync()
        {
            var tasks = await _repository.GetAllAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            return task != null ? MapToDto(task) : null;
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            // Validar DTO
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
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
            // Validar DTO
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Obter tarefa existente
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }

            // Validar regra de negócio: CompletedAt não pode ser anterior a CreatedAt
            if (dto.CompletedAt.HasValue && dto.CompletedAt < task.CreatedAt)
            {
                throw new ValidationException("A data de conclusão não pode ser anterior à data de criação.");
            }

            // Atualizar propriedades
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.CompletedAt = dto.CompletedAt;

            // Persistir
            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();

            return MapToDto(task);
        }
        public async Task DeleteAsync(int id)
        {
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
