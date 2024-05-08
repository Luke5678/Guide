using FluentValidation;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommandValidator : AbstractValidator<CreateAttractionCommand>
{
    public CreateAttractionCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Atrakcja musi posiadać nazwę");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Atrakcja musi posiadać opis");
        RuleFor(x => x.Categories).NotNull();
    }
}