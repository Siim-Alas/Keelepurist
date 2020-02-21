using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class AdminSubmittedTestDetailModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public AdminSubmittedTestDetailModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }
        public string TestContent { get; set; }
        public async Task OnGetAsync(string submittedTestName)
        {
            if (!string.IsNullOrWhiteSpace(submittedTestName))
            {
                try
                {
                    var blob = _azureStorageService.GetBlobFromContainer("submittedtests", submittedTestName);
                    TestContent = await blob.DownloadTextAsync();
                }
                catch
                {

                }
            }
        }
    }
}