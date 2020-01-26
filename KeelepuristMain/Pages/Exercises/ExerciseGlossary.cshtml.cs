using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class ExerciseGlossaryModel : PageModel
    {
        public List<string> SubDirectoryNames { get; set; }
        public List<string> ExerciseNames { get; set; }
        public void OnGet(string subDirectory)
        {
            // Currently only supports ONE level of subdirectories!!!

            var subDirectoryPaths = System.IO.Directory.GetDirectories($"wwwroot/StaticContent/exercises/{subDirectory}", "*", System.IO.SearchOption.TopDirectoryOnly);
            SubDirectoryNames = subDirectoryPaths.Select((e) => e.Split('/').Last()).ToList();

            var exercisePaths = System.IO.Directory.GetFiles($"wwwroot/StaticContent/exercises/{subDirectory}");
            ExerciseNames = exercisePaths.Select((e) => e.Split('/').Last()).ToList();
        }
    }
}