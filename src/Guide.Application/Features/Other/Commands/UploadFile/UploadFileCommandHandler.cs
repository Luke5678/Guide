using Guide.Application.Common.Services;
using MediatR;
using Serilog;

namespace Guide.Application.Features.Other.Commands.UploadFile;

public class UploadFileCommandHandler(BlobService blobService) : IRequestHandler<UploadFileCommand, string?>
{
    public async Task<string?> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var guid = Guid.NewGuid().ToString();
            var filePath = $"{guid}/{request.File.FileName}";
            
            var stream = request.File.OpenReadStream();
            var uri = await blobService.UploadFileAsync(stream, filePath);

            return uri;
        }
        catch (Exception ex)
        {
            Log.Error($"Upload file: {request.File.FileName} Error: {ex.Message}");
        }

        return null;
    }
}