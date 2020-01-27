using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeelepuristMain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeelepuristMain
{
    public class EtteytlusModel : PageModel
    {
        private readonly Random _rnd;
        public EtteytlusModel()
        {
            _rnd = new Random();
        }

        public int WordId { get; set; }
        public string WavFileString { get; set; }
        public BlankSpaceModel BlankSpace { get; set; }

        public IActionResult OnGet()
        {
            //TODO: implement proper algorithm for searching since lines and indexes don't match for later files.
            var rndNum = _rnd.Next(1, 28);

            SetPropertiesFromWordId(rndNum);

            return Page();
        }
        private void SetPropertiesFromWordId(int wordId)
        {
            WordId = wordId;

            var wordIdString = wordId.ToString().PadLeft(5, '0');
            WavFileString = $"psv_{wordIdString}.wav";

            var line = System.IO.File.ReadLines("wwwroot/StaticContent/soundpack/soundpack.txt").Skip(wordId - 1).Take(1).First();
            BlankSpace = new BlankSpaceModel(new string((from c in line.Split('\t').Last()
                               where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                               select c).ToArray()));
        }
    }
}