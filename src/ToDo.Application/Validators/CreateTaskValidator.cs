using FluentValidation;
using ToDo.Application.Dtos;

namespace ToDo.Application.Validators
{
    /// <summary>
    /// Validador para CreateTaskDto
    /// Aplica: FluentValidation para validações robustas
    /// </summary>
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskValidator()
        {
            // Validar Title
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("O título é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O título não pode ter mais de 100 caracteres.");
        }
    }
}
