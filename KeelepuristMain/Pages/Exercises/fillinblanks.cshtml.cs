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
        private readonly IAzureStorageService _azureStorageService;
        public FillInBlanksModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }

        public ExerciseModel Exercise { get; set; } = new ExerciseModel();

        public async Task<IActionResult> OnGetAsync(string exercisePath)
        {
            try
            {
                var blob = _azureStorageService.GetBlobFromContainer("exerciseswithblanks", exercisePath);

                string rawContent = await blob.DownloadTextAsync();

                Exercise.PopulateFromString(rawContent);
                return Page();
            }
            catch
            {
                return Page();
            }
        }
    }
}