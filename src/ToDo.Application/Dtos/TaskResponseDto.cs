using ToDo.Domain.Enums;
using ETaskStatus = ToDo.Domain.Enums.ETaskStatus;

namespace ToDo.Application.Dtos
{
    /// <summary>
    /// DTO para retornar uma tarefa (resposta da API)
    /// Contém todos os dados da tarefa para o cliente
    /// </summary>
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; } 

        public ETaskStatus Status { get; set; }
    }
}
