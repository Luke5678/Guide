using FluentValidation;

namespace Guide.Application.Features.Attractions.Commands.UpdateAttraction;

public class UpdateAttractionCommandValidator : AbstractValidator<UpdateAttractionCommand>
{
    public UpdateAttractionCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Atrakcja musi posiadać nazwę");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Atrakcja musi posiadać opis");
        RuleFor(x => x.Categories).NotNull();
    }
}