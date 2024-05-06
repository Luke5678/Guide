using FluentValidation;
using Guide.Domain.Entities;

namespace Guide.Common.Validators;

public class AttractionValidator : AbstractValidator<Attraction>
{
    public AttractionValidator()
    {
        // RuleFor(x => x.Name).NotEmpty().WithMessage("Nazwa jest wymagana.");
        // RuleFor(x => x.Category).NotEmpty().WithMessage("Kategoria jest wymagana.");
        // RuleFor(x => x.Description).NotEmpty().WithMessage("Opis jest wymagany.");
    }
}