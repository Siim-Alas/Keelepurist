using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class ExerciseGlossaryModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public ExerciseGlossaryModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }


        public List<string> BlobPaths { get; private set; }

        public void OnGet()
        {
            try
            {
                BlobPaths = _azureStorageService.ListBlobPathsFromContainer("exerciseswithblanks");
            }
            catch
            {

            }
        }
    }
}