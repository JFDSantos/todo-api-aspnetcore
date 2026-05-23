using ToDo.Domain.Enums;
using ETaskStatus = ToDo.Domain.Enums.ETaskStatus;

namespace ToDo.Application.Dtos
{
    /// <summary>
    /// DTO para criar uma nova tarefa
    /// Contém apenas os campos que o cliente fornece
    /// </summary>
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
