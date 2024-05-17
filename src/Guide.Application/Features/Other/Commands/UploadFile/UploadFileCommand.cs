using MediatR;
using Microsoft.AspNetCore.Http;

namespace Guide.Application.Features.Other.Commands.UploadFile;

public class UploadFileCommand : IRequest<string?>
{
    public IFormFile File { get; set; } = null!;
}