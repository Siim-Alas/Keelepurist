using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class AdminTestGlossaryModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public AdminTestGlossaryModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }

        public List<string> TestExercises { get; set; }
        public void OnGet()
        {
            try
            {
                TestExercises = _azureStorageService.ListBlobPathsFromContainer("testexercises");
            }
            catch
            {

            }
        }
    }
}