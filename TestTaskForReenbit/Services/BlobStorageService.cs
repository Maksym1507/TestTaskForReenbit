using Azure.Storage.Blobs;
using TestTaskForReenbit.Services.Abstractions;

namespace TestTaskForReenbit.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public const string BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=stillarionovwesteurope;AccountKey=owXmBTy7yetyrne5W9xCtjJqrdle9SgSC3H4qYd1ENDSUmyWcSovh+b48g6gxuyg01FPwzRMltBE+ASt3QU50Q==;EndpointSuffix=core.windows.net";
        public const string BlobStorageContainerName = "uploadedfiles";

        public async Task<string> AddFileToBlobStorageAsync(string email, string fileName, IFormFile formFile)
        {
            var blobStorageContainer = new BlobContainerClient(BlobStorageConnectionString, BlobStorageContainerName);
            var blob = blobStorageContainer.GetBlobClient(fileName);

            if (!blob.Exists())
            {
                var uploadedFileStream = formFile.OpenReadStream();

                using (uploadedFileStream)
                {
                    IDictionary<string, string> metadata = new Dictionary<string, string>
                    {
                        { "Recipient", email }
                    };
                    
                    var uploadFileToBlobStorageResponse = await blob.UploadAsync(formFile.OpenReadStream());
                    await blob.SetMetadataAsync(metadata);

                    if (uploadFileToBlobStorageResponse.GetRawResponse().Status == 201)
                    {
                        return "File has been added to BLOB Storage";
                    }
                }
            }

            return "The specified blob already exists";
        }
    }
}
