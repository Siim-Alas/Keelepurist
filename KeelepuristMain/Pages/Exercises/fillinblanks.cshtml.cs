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

        public ExerciseModel Exercise { get; set; }

        public IActionResult OnGet(string exerciseName)
        {
            if (string.IsNullOrEmpty(exerciseName)) {
                var exerciseNames = System.IO.Directory.GetFiles("wwwroot/StaticContent/exercises/").ToList();
                exerciseNames = exerciseNames.Select((e) => e.Split('/').Last()).ToList();

                var rndNum = _rnd.Next(0, exerciseNames.Count - 1);

                var randomRawText = System.IO.File.ReadAllText($"wwwroot/StaticContent/exercises/{exerciseNames[rndNum]}");

                Exercise = new ExerciseModel() { Name = exerciseNames[rndNum] };
                Exercise.PopulateFromString(randomRawText);
                return Page();
            }
            var rawText = System.IO.File.ReadAllText($"wwwroot/StaticContent/exercises/{exerciseName}");
            Exercise = new ExerciseModel() { Name = exerciseName };
            Exercise.PopulateFromString(rawText);
            return Page();
        }
    }
}