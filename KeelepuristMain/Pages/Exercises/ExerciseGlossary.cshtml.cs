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
        public List<string> S3objectPaths { get; private set; } = new List<string>();

        public void OnGet()
        {
        }
    }
}