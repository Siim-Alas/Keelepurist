using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Models
{
    public class BlankSpaceModel
    {
        public List<string> CorrectAnswers { get; set; }
        public BlankSpaceModel()
        {

        }
        public BlankSpaceModel(string rawText)
        {
            CorrectAnswers = rawText.Split('|').ToList();
        }
    }
}
