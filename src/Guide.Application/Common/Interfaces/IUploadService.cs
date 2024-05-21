namespace Guide.Application.Common.Interfaces;

public interface IUploadService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string url);
}