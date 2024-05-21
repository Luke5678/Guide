using Guide.Application.Common.Interfaces;
using MediatR;
using Serilog;

namespace Guide.Application.Features.Other.Commands.UploadFile;

public class UploadFileCommandHandler(IUploadService uploadService) : IRequestHandler<UploadFileCommand, string?>
{
    public async Task<string?> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stream = request.File.OpenReadStream();
            return await uploadService.UploadFileAsync(stream, request.File.FileName);
        }
        catch (Exception ex)
        {
            Log.Error($"Upload file: {request.File.FileName} Error: {ex.Message}");
        }

        return null;
    }
}