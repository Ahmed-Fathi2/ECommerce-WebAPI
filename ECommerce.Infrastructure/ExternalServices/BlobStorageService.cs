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


            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_options.ContainerName);


  
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            //PublicAccessType.None
            //PublicAccessType.Blob;
            //PublicAccessType.BlobContainer;


            var uniqueFileName = GenerateUniqueFileName(fileName);

            // Get a reference to the blob (file) you want to upload
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

            //if you want to download the file if  url  Called
            //you need to set the content type in the blob properties to be application/octet-stream [Default content type in upload]

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

            await blobClient.UploadAsync(fileData, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
                // default content type in upload is application/octet-stream,
                // //if you want to open the file in browser you need to set the content type to be the actual content type of the file
                // (e.g., image/jpeg for jpg files)

            });


            var blobUrl = blobClient.Uri.ToString();

            return blobUrl;
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();

            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName)
                             .Replace(" ", "_")
                             .ToLower();

            var uniqueName = $"{nameWithoutExt}-{Guid.NewGuid()}{extension}";

            return uniqueName;
        }

        public string? GetBlobSasUrl(string? blobUrl)
        {

            if (blobUrl == null) return null;


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
/*
using Azure.Storage;
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
        private readonly StorageSharedKeyCredential _credential;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IOptions<BlobStorageSettings> options)
        {
            _options = options.Value;

            // ✅ Build credential once from ConnectionString — works for both Azure & Azurite
            _credential = BuildCredential(_options.ConnectionString);
            _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        }

        public async Task<string> UploadToBlobAsync(Stream fileData, string fileName, string contentType)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);

            // ✅ Private container (None, not Blob)
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            var uniqueFileName = GenerateUniqueFileName(fileName);
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

            await blobClient.UploadAsync(fileData, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            return blobClient.Uri.ToString();
        }

        public string? GetBlobSasUrl(string? blobUrl)
        {
            if (blobUrl == null) return null;

            var blobName = GetBlobNameFromUrl(blobUrl);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _options.ContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5), // ✅ clock skew buffer
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // ✅ Generate SAS token using the shared credential
            var sasToken = sasBuilder.ToSasQueryParameters(_credential).ToString();

            return $"{blobUrl}?{sasToken}";
        }

        // ✅ Extracts the correct credential from any ConnectionString format
        
        private static StorageSharedKeyCredential BuildCredential(string connectionString)
        {
            // Azurite shorthand
            if (connectionString.Equals("UseDevelopmentStorage=true", StringComparison.OrdinalIgnoreCase))
            {
                return new StorageSharedKeyCredential(
                    "devstoreaccount1",
                    "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
                );
            }

            // Azure full connection string: "AccountName=xxx;AccountKey=yyy;..."
            var parts = connectionString
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split('=', 2))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0].Trim(), p => p[1].Trim());

            if (!parts.TryGetValue("AccountName", out var accountName) ||
                !parts.TryGetValue("AccountKey", out var accountKey))
            {
                throw new InvalidOperationException(
                    "ConnectionString must contain AccountName and AccountKey.");
            }

            return new StorageSharedKeyCredential(accountName, accountKey);
        }
        

        private static string GetBlobNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return uri.Segments.Last();
        }

        private static string GenerateUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName)
                .Replace(" ", "_")
                .ToLower();
            return $"{nameWithoutExt}-{Guid.NewGuid()}{extension}";
        }
    }
}
*/