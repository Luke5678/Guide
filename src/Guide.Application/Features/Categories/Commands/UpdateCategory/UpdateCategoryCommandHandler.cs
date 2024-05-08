using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var category = await _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (category == null)
        {
            return null!;
        }

        category.Translations.First().Name = request.Name;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}