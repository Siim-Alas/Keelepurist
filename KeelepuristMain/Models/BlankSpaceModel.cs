using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Models
{
    public class BlankSpaceModel
    {
        public List<string> CorrectAnswers { get; set; } = new List<string>();
        public string UserAnswer { get; set; } = "";
        public bool UserAnsweredRight
        {
            get
            {
                return CorrectAnswers.Contains(UserAnswer);
            }
        }
        public BlankSpaceModel()
        {

        }
        public BlankSpaceModel(string rawText)
        {
            CorrectAnswers = rawText.Split('|').ToList();
        }
    }
}
