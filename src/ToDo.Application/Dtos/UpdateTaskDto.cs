using ToDo.Domain.Enums;
using ETaskStatus = ToDo.Domain.Enums.ETaskStatus;

namespace ToDo.Application.Dtos
{
    /// <summary>
    /// DTO para atualizar uma tarefa existente
    /// Contém todos os campos que podem ser alterados
    /// </summary>
    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ETaskStatus Status { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
