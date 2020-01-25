using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string Word { get; set; }
        public AnswerTypes UserAnswer { get; set; } = AnswerTypes.None;

        public enum AnswerTypes
        {
            Right,
            Wrong,
            None
        };

        public IActionResult OnGet()
        {
            UserAnswer = AnswerTypes.None;

            //TODO: implement proper algorithm for searching since lines and indexes don't match for later files.
            var rndNum = _rnd.Next(1, 28);

            SetPropertiesFromWordId(rndNum);

            return Page();
        }
        public IActionResult OnPost(int wordId, string input)
        {
            SetPropertiesFromWordId(wordId);

            if (input == Word)
            {
                UserAnswer = AnswerTypes.Right;
                return Page();
            }
            UserAnswer = AnswerTypes.Wrong;
            return Page();
        }
        private void SetPropertiesFromWordId(int wordId)
        {
            WordId = wordId;

            var wordIdString = wordId.ToString().PadLeft(5, '0');
            WavFileString = $"psv_{wordIdString}.wav";

            var line = System.IO.File.ReadLines("wwwroot/soundpack/soundpack.txt").Skip(wordId - 1).Take(1).First();
            Word = new string((from c in line.Split('\t').Last()
                               where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                               select c).ToArray());
        }
    }
}