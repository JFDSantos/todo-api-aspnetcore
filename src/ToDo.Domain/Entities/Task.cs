using System.ComponentModel.DataAnnotations;
using ToDo.Domain.Enums;
using ETaskStatus = ToDo.Domain.Enums.ETaskStatus;

namespace ToDo.Domain.Entities
{
    /// <summary>
        /// Entidade TodoTask - Representa uma tarefa no sistema
    /// </summary>
    public class TodoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres")]

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
        public ETaskStatus Status { get; set; }

        public TodoTask()
        {
            CreatedAt = DateTime.Now;
            Status = ETaskStatus.Pendente;
        }

        public TodoTask(string title, string? description = null) : this()
        {
            Title = title;
            Description = description;
        }
    }
}
