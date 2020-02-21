using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class AdminSubmittedTestsGlossaryModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public AdminSubmittedTestsGlossaryModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }

        public List<string> SubmittedTests { get; set; }
        public void OnGet()
        {
            SubmittedTests = _azureStorageService.ListBlobPathsFromContainer("submittedtests");
        }
    }
}