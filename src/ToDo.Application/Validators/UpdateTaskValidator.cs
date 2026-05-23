using FluentValidation;
using ToDo.Application.Dtos;

namespace ToDo.Application.Validators
{
    /// <summary>
    /// Validador para UpdateTaskDto
    /// Aplica: FluentValidation para validações robustas
    /// </summary>
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            // Validar Title
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("O título é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O título não pode ter mais de 100 caracteres.");

            // Validar CompletedAt
            RuleFor(x => x.CompletedAt)
                .Null()
                .Unless(x => x.CompletedAt.HasValue)
                .WithMessage("Data de conclusão inválida.")
                .When(x => x.CompletedAt.HasValue);
        }
    }
}
