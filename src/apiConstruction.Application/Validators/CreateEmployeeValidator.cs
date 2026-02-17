using apiConstruction.Application.DTOs.Requests;
using FluentValidation;

namespace apiConstruction.Application.Validators;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(200).WithMessage("El email no puede exceder 200 caracteres");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("La fecha de contratación es requerida")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de contratación no puede ser futura");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("El salario debe ser mayor a cero");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("El departamento es requerido");
    }
}
