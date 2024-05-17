using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Guide.Application.Features.Other.Commands.UploadFile;

public class UploadFileCommandHandler(IHostEnvironment env) : IRequestHandler<UploadFileCommand, string?>
{
    public async Task<string?> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid().ToString();
        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "uploads", guid);

        if (env.IsDevelopment())
        {
            basePath = basePath.Replace(@"bin\Debug\net8.0\", "");
        }

        try
        {
            Directory.CreateDirectory(basePath);

            var path = Path.Combine(basePath, request.File.FileName);
            await using FileStream fs = new(path, FileMode.Create);
            await request.File.OpenReadStream().CopyToAsync(fs, cancellationToken);

            return $"{guid}/{request.File.FileName}";
        }
        catch (Exception ex)
        {
            Log.Error($"Upload file: {request.File.FileName} Error: {ex.Message}");
        }

        return null;
    }
}