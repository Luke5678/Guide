using Guide.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Guide.Application.Common.Services;

public class UploadService(IWebHostEnvironment env) : IUploadService
{
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var guid = Guid.NewGuid().ToString();
        var directory = Path.Combine(GetUploadsPaths(), guid);

        Directory.CreateDirectory(directory);

        await using FileStream fs = new(Path.Combine(directory, fileName), FileMode.Create);
        await fileStream.CopyToAsync(fs);

        return $"/uploads/{guid}/{fileName}";
    }

    public Task DeleteFileAsync(string url)
    {
        try
        {
            var filePath = url.Split("uploads/").Last();
            File.Delete(Path.Combine(GetUploadsPaths(), filePath));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Task.CompletedTask;
    }

    private string GetUploadsPaths()
    {
        var uploadsPath = Path.Combine(env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);
        return uploadsPath;
    }
}