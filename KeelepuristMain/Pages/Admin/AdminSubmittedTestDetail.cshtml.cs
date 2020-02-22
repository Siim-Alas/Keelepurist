using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [BindProperty]
        [Required]
        public string SubmissionName { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string[] TestContentArray { get; set; }
        public async Task<IActionResult> OnGetAsync(string submittedTestName)
        {
            if (!string.IsNullOrWhiteSpace(submittedTestName))
            {
                try
                {
                    var blob = _azureStorageService.GetBlobFromContainer("submittedtests", submittedTestName);
                    await blob.FetchAttributesAsync();
                    var blobText = await blob.DownloadTextAsync();

                    SubmissionName = submittedTestName;
                    LastModified = (DateTimeOffset)blob.Properties.LastModified;
                    TestContentArray = blobText.Split("///");
                }
                catch
                {
                    
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var blob = _azureStorageService.GetBlobFromContainer("submittedtests", SubmissionName);
                await blob.DeleteIfExistsAsync();

                return RedirectToPage("./AdminSubmittedTestsGlossary");
            }
            catch
            {
                return Page();
            }
        }
    }
}