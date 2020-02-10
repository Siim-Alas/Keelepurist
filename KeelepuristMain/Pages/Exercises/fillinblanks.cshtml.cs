using KeelepuristMain.Models;
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
        public ExerciseModel Exercise { get; set; } = new ExerciseModel();

        public IActionResult OnGet(string exerciseName)
        {
            // var s3Obj = await _S3Service.GetObjectFromS3Async("keelepurist", exerciseName);
            // var responseStream = s3Obj.ResponseStream;
            string rawContent = "";
            Exercise.PopulateFromString(rawContent);
            return Page();
        }
    }
}