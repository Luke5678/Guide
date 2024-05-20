using Guide.Application.Features.Other.Commands.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers;

[Route("api/upload")]
public class UploadController : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post()
    {
        if (!Request.HasFormContentType)
        {
            return BadRequest();
        }

        var form = await Request.ReadFormAsync();

        if (form.Files.Count == 0 || form.Files[0].Length == 0)
        {
            return BadRequest();
        }

        var result = await Mediator.Send(new UploadFileCommand
        {
            File = form.Files[0]
        });

        return result == null
            ? BadRequest()
            : Ok(new { location = result });
    }
}