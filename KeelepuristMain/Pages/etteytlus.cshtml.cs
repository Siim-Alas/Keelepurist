using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeelepuristMain.Models;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class EtteytlusModel : PageModel
    {
        private readonly Random _rnd;
        private readonly IS3Service _S3Service;
        public EtteytlusModel(IS3Service service)
        {
            _rnd = new Random();
            _S3Service = service;
        }

        public string WavFileURL { get; set; }
        public BlankSpaceModel BlankSpace { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var rndNum = _rnd.Next(1, 2025);

            await SetPropertiesFromWordId(rndNum);

            return Page();
        }
        private async Task SetPropertiesFromWordId(int wordId)
        {
            var s3Obj = await _S3Service.GetObjectFromS3Async("keelepurist", "soundpack/soundpackcatalog.txt");
            var responseStream = s3Obj.ResponseStream;
            string fileCatalog;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.Unicode))
            {
                fileCatalog = reader.ReadToEnd();
            }
            var lineStringArray = fileCatalog.Split("\n")[wordId - 1].Split("\t");

            WavFileURL = _S3Service.GetPreSignedURLFromS3("keelepurist", "soundpack/" + lineStringArray[0]);

            BlankSpace = new BlankSpaceModel(new string((from c in lineStringArray.Last()
                                                         where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                                                         select c)
                                                         .ToArray()));
        }
    }
}