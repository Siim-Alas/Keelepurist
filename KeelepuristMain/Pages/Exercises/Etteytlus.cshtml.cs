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
        private readonly IAzureStorageService _azureStorageService;
        private readonly Random _rnd;
        public EtteytlusModel(IAzureStorageService service)
        {
            _azureStorageService = service;
            _rnd = new Random();
        }

        public string WavFileURL { get; set; }
        public BlankSpaceModel BlankSpace { get; set; }

        public IActionResult OnGetAsync()
        {
            // NOT all sound files are uploaded to Azure Blob Storage!
            var rndNum = _rnd.Next(1, 1000);

            SetPropertiesFromWordId(rndNum);

            return Page();
        }
        private void SetPropertiesFromWordId(int wordId)
        {
            var lineStringArray = System.IO.File.ReadLines("wwwroot/StaticContent/soundpackcatalog.txt")
                                                .Skip(wordId - 1).Take(1).First()
                                                .Split("\t");

            var blob = _azureStorageService.GetBlobFromContainer("soundpack", lineStringArray[0]);
            WavFileURL = _azureStorageService.GetServiceSASUriForBlob(blob);

            BlankSpace = new BlankSpaceModel(new string((from c in lineStringArray.Last()
                                                         where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                                                         select c)
                                                         .ToArray()));
        }
    }
}