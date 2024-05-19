using Azure.Core.Diagnostics;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Guide.Application.Common.Services;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobService(IConfiguration configuration)
    {
        _containerName = configuration["AzureStorage:ContainerName"]!;
        _blobServiceClient = new BlobServiceClient(
            new Uri($"https://{configuration["AzureStorage:StorageName"]}.blob.core.windows.net"),
            new DefaultAzureCredential()
        );
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string filePath)
    {
        // using var listener = AzureEventSourceListener.CreateConsoleLogger();

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(filePath);
        await blobClient.UploadAsync(fileStream, true);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string filePath)
    {
        using var listener = AzureEventSourceListener.CreateConsoleLogger();

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(filePath);
        await blobClient.DeleteIfExistsAsync();
    }
}