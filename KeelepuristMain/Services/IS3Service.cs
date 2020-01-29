﻿using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Services
{
    public interface IS3Service
    {
        Task<ListObjectsResponse> ListObjectsFromS3Async(string bucketName);
        Task<GetObjectResponse> GetObjectFromS3Async(string bucketName, string key);
    }
}