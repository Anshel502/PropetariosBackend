using FluentValidation;
using Backend.DTOs;

namespace Backend.Validators;

public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
{
    public BrandUpdateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la marca es obligatorio.")
            .Length(2, 20).WithMessage("El nombre de la marca debe tener entre 2 y 20 caracteres.");
    }
}