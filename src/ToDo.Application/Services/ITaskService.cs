using ToDo.Application.Dtos;

namespace ToDo.Application.Services
{
    /// <summary>
    /// Interface para o serviço de tarefas
    /// </summary>
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllAsync();

        Task<TaskResponseDto?> GetByIdAsync(int id);

        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);

        Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto);

        Task<TaskResponseDto> StartAsync(int id);

        Task<TaskResponseDto> CompleteAsync(int id);

        Task<TaskResponseDto> ReopenAsync(int id);

        Task DeleteAsync(int id);
    }
}
