using KeelepuristMain.Models;
using KeelepuristMain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace KeelepuristMain
{
    public class FillInBlanksModel : PageModel
    {
        private readonly IS3Service _S3Service;
        public FillInBlanksModel(IS3Service service)
        {
            _S3Service = service;
        }

        public ExerciseModel Exercise { get; set; } = new ExerciseModel();

        public async Task<IActionResult> OnGetAsync(string exerciseName)
        {
            var s3Obj = await _S3Service.GetObjectFromS3Async("keelepurist", exerciseName);
            var responseStream = s3Obj.ResponseStream;
            string rawContent;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.Unicode))
            {
                rawContent = reader.ReadToEnd();
            }
            Exercise.PopulateFromString(rawContent);
            return Page();
        }
    }
}