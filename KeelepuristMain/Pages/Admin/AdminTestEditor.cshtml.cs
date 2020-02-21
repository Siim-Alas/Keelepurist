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
                    await blob.FetchAttributesAsync();

                    TestName = blob.Name;
                    TestContent = await blob.DownloadTextAsync();
                    IsPublic = (blob.Metadata["ispublic"] == "True");
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
            var blob = _azureStorageService.GetBlobFromContainer("testexercises", TestName);
            await blob.UploadTextAsync(TestContent);
            blob.Metadata["ispublic"] = IsPublic.ToString();
            await blob.SetMetadataAsync();

            return RedirectToPage("./AdminTestGlossary");
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var blob = _azureStorageService.GetBlobFromContainer("testexercises", TestName);
            await blob.DeleteIfExistsAsync();
            return RedirectToPage("./AdminTestGlossary");
        }
    }
}