using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlobStorageFunction
{
    public class BlobStorageFunction
    {
        [FunctionName("EmailNotificationAfterFileHasBeenAddedToBlobStorage")]
        public async Task Run([BlobTrigger("uploadedfiles/{name}", Connection = "storageAccount")]Stream myBlob, string name,
        IDictionary<string, string> metaData,
        ILogger log)
        {
            var recipient = metaData.FirstOrDefault(f => f.Key == "Recipient");

            if (recipient.Key != null && recipient.Value != null)
            {
                var client = new SendGridClient(Environment.GetEnvironmentVariable("AzureWebJobsSendGridApiKey"));

                var message = new SendGridMessage();

                message.SetFrom(new EmailAddress(Environment.GetEnvironmentVariable("EmailSender")));
                message.AddTo(new EmailAddress(recipient.Value));
                message.SetSubject($"File Uploaded");
                message.AddContent(MimeType.Text, $"The file with name {name} is successfully uploaded to Azure Blob Storage.");

                try
                {
                    var response = await client.SendEmailAsync(message);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        log.LogInformation($"The file with name {name} is successfully uploaded to Azure Blob Storage.");
                    }
                }
                catch (Exception ex)
                {
                    log.LogError($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
