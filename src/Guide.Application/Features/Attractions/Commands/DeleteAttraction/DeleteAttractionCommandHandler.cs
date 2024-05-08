using Guide.Infrastructure;
using MediatR;

namespace Guide.Application.Features.Attractions.Commands.DeleteAttraction;

public class DeleteAttractionCommandHandler : IRequestHandler<DeleteAttractionCommand, bool>
{
    private readonly GuideDbContext _dbContext;

    public DeleteAttractionCommandHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteAttractionCommand request, CancellationToken cancellationToken)
    {
        var attraction = await _dbContext.Attractions.FindAsync(request.Id, cancellationToken);

        if (attraction == null)
        {
            return false;
        }

        _dbContext.Remove(attraction);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}