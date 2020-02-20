using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Services
{
    public interface IAzureStorageService
    {
        public CloudBlobContainer GetCloudBlobContainer(string containerName);
        public List<string> ListBlobPathsFromContainer(string containerName, string prefix = "");
        public CloudBlockBlob GetBlobFromContainer(string containerName, string blobName);
        public string GetServiceSASUriForBlob(CloudBlob blob);
    }
}
