using KeelepuristMain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace KeelepuristMain
{
    public class FillInBlanksModel : PageModel
    {
        private readonly Random _rnd;
        public FillInBlanksModel()
        {
            _rnd = new Random();
        }

        [BindProperty]
        public ExerciseModel Exercise { get; set; }

        public IActionResult OnGet(string exerciseName)
        {
            if (string.IsNullOrEmpty(exerciseName)) {
                var exercises = System.IO.Directory.GetFiles("wwwroot/StaticContent/exercises/");
                var rndNum = _rnd.Next(0, exercises.Length - 1);

                var randomRawText = System.IO.File.ReadAllText(exercises[rndNum]);

                Exercise = new ExerciseModel() { Name = exercises[rndNum] };
                Exercise.PopulateFromString(randomRawText);
                return Page();
            }
            var rawText = System.IO.File.ReadAllText(exerciseName);
            Exercise = new ExerciseModel() { Name = exerciseName };
            Exercise.PopulateFromString(rawText);
            return Page();
        }
        public IActionResult OnPost(string exercise)
        {
            Exercise = JsonConvert.DeserializeObject<ExerciseModel>(exercise);

            for (var i = 0; i < Exercise.BlankSpaces.Count; i++)
            {
                Exercise.BlankSpaces[i].UserAnswer = Request.Form[i.ToString()];
            }

            if (Exercise.BlankSpaces.TrueForAll((e) => e.UserAnsweredRight))
            {
                return RedirectToPage("../index");
            }
            return Page();
        }
    }
}