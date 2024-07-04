using Guide.Application.Features.Reviews.Commands;
using Guide.Application.Features.Reviews.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers;

[Route("api/review")]
public class ReviewController : BaseController
{
    [HttpGet("{attractionId:int}")]
    public async Task<IActionResult> Get(int attractionId = 0)
    {
        return Ok(await Mediator.Send(new GetAttractionReviewsQuery
        {
            AttractionId = attractionId
        }));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(MakeReviewCommand request)
    {
        await Mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{reviewId:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int reviewId)
    {
        await Mediator.Send(new DeleteReviewCommand
        {
            ReviewId = reviewId
        });
        return Ok();
    }
}