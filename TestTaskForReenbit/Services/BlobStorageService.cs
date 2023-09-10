using Azure.Storage.Blobs;
using TestTaskForReenbit.Services.Abstractions;

namespace TestTaskForReenbit.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public const string BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=stillarionovwesteurope;AccountKey=owXmBTy7yetyrne5W9xCtjJqrdle9SgSC3H4qYd1ENDSUmyWcSovh+b48g6gxuyg01FPwzRMltBE+ASt3QU50Q==;EndpointSuffix=core.windows.net";
        public const string BlobStorageContainerName = "uploadedfiles";

        public async Task<bool> AddFileToBlobStorageAsync(string email, string fileName, IFormFile formFile)
        {
            var blobStorageContainer = new BlobContainerClient(BlobStorageConnectionString, BlobStorageContainerName);
            var blob = blobStorageContainer.GetBlobClient(fileName);

            var uploadedFileStream = formFile.OpenReadStream();

            using (uploadedFileStream)
            {
                var uploadFileToBlobStorageResponse = await blob.UploadAsync(formFile.OpenReadStream());

                if (uploadFileToBlobStorageResponse.GetRawResponse().Status == 201)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
