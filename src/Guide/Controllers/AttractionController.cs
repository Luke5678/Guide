using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Guide.Application.Features.Attractions.Queries.GetAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttractions;

namespace Guide.Controllers;

[Route("api/attraction")]
public class AttractionController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(int page = 0, int limit = 0, string? orderBy = "", string? search = "")
    {
        return Ok(await Mediator.Send(new GetAttractionsQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search,
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        }));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await Mediator.Send(new GetAttractionQuery
        {
            Id = id, LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        }));
    }
}