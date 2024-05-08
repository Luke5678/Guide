using AutoMapper;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;

namespace Guide.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto?>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Translations = LanguageCodes.List.Select(x => new CategoryTranslation
            {
                LanguageCode = x,
                Name = request.Name,
            }).ToArray()
        };

        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}