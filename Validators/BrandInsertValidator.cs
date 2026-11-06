using FluentValidation;
using Backend.DTOs;

namespace Backend.Validators;

public class BrandInsertValidator : AbstractValidator<BrandInsertDto>
{
    public BrandInsertValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la marca es obligatorio.")
            .Length(2, 20).WithMessage("El nombre de la marca debe tener entre 2 y 20 caracteres.");
    }
}