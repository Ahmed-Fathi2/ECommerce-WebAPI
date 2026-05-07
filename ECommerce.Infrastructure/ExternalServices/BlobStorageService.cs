using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.ExternalServices
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobStorageSettings _options;

        public BlobStorageService(IOptions<BlobStorageSettings> options)
        {
            _options = options.Value;
        }

     

        public async Task<string> UploadToBlobAsync(Stream fileData, string fileName, string contentType)
        {
         
            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);

            // Get a reference to the container you want to upload to 
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_options.ContainerName);

            // Get a reference to the blob (file) you want to upload
            var blobClient = blobContainerClient.GetBlobClient(fileName);


            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType }; 

            await blobClient.UploadAsync(fileData, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });


            var blobUrl = blobClient.Uri.ToString();

            return blobUrl;
        }
        public string? GetBlobSasUrl(string? blobUrl)
        {

            if (blobUrl == null) return null;

            // form sas token 

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _options.ContainerName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
                BlobName = GetBlobNameFrmUrl(blobUrl)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);

            var sasToken = sasBuilder
                .ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _options.AccountKey));



            return $"{blobUrl}?{sasToken}";
        }

        private string GetBlobNameFrmUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            var blobName = uri.Segments.Last();
            return blobName;
        }
    }
}
