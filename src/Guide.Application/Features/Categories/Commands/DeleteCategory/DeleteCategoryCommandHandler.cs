using Guide.Infrastructure;
using MediatR;

namespace Guide.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly GuideDbContext _dbContext;

    public DeleteCategoryCommandHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FindAsync(request.Id, cancellationToken);

        if (category == null)
        {
            return false;
        }

        _dbContext.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}