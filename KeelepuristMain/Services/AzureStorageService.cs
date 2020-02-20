using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        public CloudBlobContainer GetCloudBlobContainer(string containerName)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            return container;
        }

        public List<string> ListBlobPathsFromContainer(string containerName, string prefix = "")
        {
            CloudBlobContainer container = GetCloudBlobContainer(containerName);
            BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(
                prefix,
                true,
                BlobListingDetails.All,
                1000,
                null,
                null, // new BlobRequestOptions(),
                null //new OperationContext()
                ).Result;

            return SearchListBlobItem(resultSegment);
        }
        private List<string> SearchListBlobItem(BlobResultSegment resultSegment)
        {
            var blobs = new List<string>();
            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name);
                }
            }
            return blobs;
        }

        public CloudBlockBlob GetBlobFromContainer(string containerName, string blobName)
        {
            return GetCloudBlobContainer(containerName).GetBlockBlobReference(blobName);
        }

        public string GetServiceSASUriForBlob(CloudBlob blob)
        {
            string sasBlobToken;
            SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(5),
                Permissions = SharedAccessBlobPermissions.Read
            };
            sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

            return $"{blob.Uri}{sasBlobToken}";
        }
    }
}
