using Azure.Storage;
using Azure.Storage.Sas;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorageFunction
{
    public class BlobStorageFunction
    {
        [FunctionName("EmailNotificationAfterFileHasBeenAddedToBlobStorage")]
        public async Task Run([BlobTrigger("uploadedfiles/{name}", Connection = "BlobStorageConnectionString")] Stream myBlob,
            string name,
            Uri uri,
            IDictionary<string,
            string> metaData,
            ILogger log)
        {
            var recipient = metaData.FirstOrDefault(f => f.Key == "Recipient");

            if (recipient.Key != null && recipient.Value != null)
            {
                var client = new SendGridClient(Environment.GetEnvironmentVariable("AzureWebJobsSendGridApiKey"));

                var message = new SendGridMessage();

                var blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = Environment.GetEnvironmentVariable("BlobStorageContainerName"),
                    BlobName = name,
                    StartsOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                blobSasBuilder.SetPermissions((BlobSasPermissions.Read));
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(Environment.GetEnvironmentVariable("StorageAccount"), Environment.GetEnvironmentVariable("BlobStorageApiKey"))).ToString();
                var sasUrl = $"{uri}?{sasToken}";

                message.SetFrom(new EmailAddress(Environment.GetEnvironmentVariable("EmailSender")));
                message.AddTo(new EmailAddress(recipient.Value));
                message.SetSubject($"File Uploaded");
                message.AddContent(MimeType.Text, $"The file with name {name} is successfully uploaded to Azure Blob Storage.\n{sasUrl}");

                try
                {
                    var response = await client.SendEmailAsync(message);

                    if (response.IsSuccessStatusCode)
                    {
                        log.LogInformation($"User with email: {recipient.Value} has been notified");
                    }
                }
                catch (Exception ex)
                {
                    log.LogError($"Error sending email: {ex.Message}");
                }
            }
            else
            {
                log.LogError($"Not founded recipient");
            }
        }
    }
}
