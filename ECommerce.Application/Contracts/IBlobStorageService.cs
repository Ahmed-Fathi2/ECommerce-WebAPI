namespace ECommerce.Application.Contracts
{
    public interface IBlobStorageService
    {
        Task<string> UploadToBlobAsync(Stream fileData, string fileName , string contentType);
        string? GetBlobSasUrl(string? blobUrl);
    }
}
