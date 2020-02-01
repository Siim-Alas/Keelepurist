using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;
        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }
        public async Task<ListObjectsResponse> ListObjectsFromS3Async(string bucketName, string prefix)
        {
            try
            {
                var request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };

                return await _client.ListObjectsAsync(request);
            }
            catch
            {
                return new ListObjectsResponse();
            }
        }

        public async Task<GetObjectResponse> GetObjectFromS3Async(string bucketName, string key)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                return await _client.GetObjectAsync(request);
            }
            catch
            {
                return new GetObjectResponse();
            }
        }

        public string GetPreSignedURLFromS3(string bucketName, string key)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    Expires = DateTime.Now.AddMinutes(5)
                };

                return _client.GetPreSignedURL(request);
            }
            catch
            {
                return "";
            }
        }
    }
}
