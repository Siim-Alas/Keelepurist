using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Services
{
    public interface IAzureStorageService
    {
        public CloudBlobContainer GetCloudBlobContainer();
        public List<string> ListBlobs();
    }
}
