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
        private readonly IAzureStorageService _asureStorageService;
        public ExerciseGlossaryModel(IAzureStorageService service)
        {
            _asureStorageService = service;
        }


        public List<string> BlobPaths { get; private set; } = new List<string>();

        public void OnGet()
        {
            BlobPaths = _asureStorageService.ListBlobPathsFromContainer("eserciseswithblanks");
        }
    }
}