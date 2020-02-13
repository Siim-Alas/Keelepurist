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
        public List<string> ListBlobPathsFromContainer(string containerName);
        public CloudBlockBlob GetBlobFromContainer(string containerName, string blobName);
    }
}
