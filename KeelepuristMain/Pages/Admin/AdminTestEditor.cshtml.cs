using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KeelepuristMain
{
    public class AdminTestEditorModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public AdminTestEditorModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }

        [BindProperty]
        [Required]
        public string TestName { get; set; }
        [BindProperty]
        public string TestContent { get; set; }
        [BindProperty]
        public bool IsPublic { get; set; }
        public async Task OnGetAsync(string blobName)
        {
            if (!string.IsNullOrWhiteSpace(blobName))
            {
                try
                {
                    var blob = _azureStorageService.GetBlobFromContainer("testexercises", blobName);
                    var testAvaliabilityAndNameArray = blob.Name.Split("/");

                    TestName = testAvaliabilityAndNameArray.Last();
                    TestContent = await blob.DownloadTextAsync();
                    IsPublic = (testAvaliabilityAndNameArray.First() == "public");
                }
                catch
                {

                }
            }
        }
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var blob = _azureStorageService.GetBlobFromContainer("testexercises",
                                                                 $"{(IsPublic ? "public" : "hidden")}/{TestName}");
            await blob.UploadTextAsync(TestContent);

            var potentialOldBlob = _azureStorageService.GetBlobFromContainer("testexercises",
                                                                             $"{(IsPublic ? "hidden" : "public")}/{TestName}");
            await potentialOldBlob.DeleteIfExistsAsync();

            return RedirectToPage("./AdminTestGlossary");
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var blob = _azureStorageService.GetBlobFromContainer("testexercises",
                                                                 $"{(IsPublic ? "public" : "hidden")}/{TestName}");
            await blob.DeleteIfExistsAsync();
            return RedirectToPage("./AdminTestGlossary");
        }
    }
}