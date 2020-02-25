using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Models;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class TestFillInBlanksModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;
        public TestFillInBlanksModel(IAzureStorageService service)
        {
            _azureStorageService = service;
        }

        public ExerciseModel Exercise { get; set; } = new ExerciseModel();

        [BindProperty]
        public bool UserAlreadySubmitted { get; set; }
        [BindProperty]
        public string TestPath { get; set; }
        [BindProperty]
        public List<string> UserAnswers { get; set; }

        public async Task<IActionResult> OnGetAsync(string testPath)
        {
            UserAlreadySubmitted = await _azureStorageService
                                         .GetBlobFromContainer(
                                         "submittedtests",
                                         $"{TestPath.Substring(TestPath.LastIndexOf("/"))}/{User.Claims.Where(c => c.Type == "name").First().Value}"
                                         ).ExistsAsync();

            if (UserAlreadySubmitted)
            {
                return Page();
            }
            var blob = _azureStorageService.GetBlobFromContainer("testexercises", testPath);
            TestPath = testPath;

            string rawContent = await blob.DownloadTextAsync();

            Exercise.PopulateFromString(rawContent);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || UserAlreadySubmitted)
            {
                return Page();
            }
            var exerciseBlob = _azureStorageService.GetBlobFromContainer("testexercises", TestPath);
            var exerciseContent = await exerciseBlob.DownloadTextAsync();
            var exerciseContentArray = exerciseContent.Split("///");

            var answer = "";

            for (var i = 0; i < exerciseContentArray.Length; i++)
            {
                // Even numbers are always text
                if (i % 2 == 0)
                {
                    answer += exerciseContentArray[i];
                }
                // Odd numbers are always blank spaces
                else
                {
                    answer += $"///{(Array.IndexOf(exerciseContentArray[i].Split("|"), UserAnswers[(i - 1) / 2]) != -1)}|{UserAnswers[(i - 1) / 2]}///";
                }
            }

            var blob = _azureStorageService.GetBlobFromContainer("submittedtests",
                                                                 $"{TestPath.Substring(TestPath.LastIndexOf("/"))}/{User.Claims.Where(c => c.Type == "name").First().Value}");
            await blob.UploadTextAsync(answer);
            return RedirectToPage("/TestExercises/TestExerciseGlossary");
        }
    }
}