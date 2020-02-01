using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class ExerciseGlossaryModel : PageModel
    {
        private readonly IS3Service _S3Service;
        public ExerciseGlossaryModel(IS3Service service)
        {
            _S3Service = service;
        }

        public List<string> S3objectPaths { get; private set; } = new List<string>();

        public async Task OnGetAsync()
        {
            var response = await _S3Service.ListObjectsFromS3Async("keelepurist", "lünkharjutused/");
            foreach (var s3Obj in response.S3Objects)
            {
                S3objectPaths.Add(s3Obj.Key);
            }
        }
    }
}