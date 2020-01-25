using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelepuristMain.Models
{
    public class ExerciseModel
    {
        public string Name { get; set; }
        public List<string> Texts { get; set; } = new List<string>();
        public List<BlankSpaceModel> BlankSpaces { get; set; } = new List<BlankSpaceModel>();

        public void PopulateFromString(string input)
        {
            var inputArray = input.Split("///");
            for (var i = 0; i < inputArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Texts.Add(inputArray[i]);
                }
                else
                {
                    BlankSpaces.Add(new BlankSpaceModel() { CorrectAnswers = inputArray[i].Split('|').ToList() });
                }
            }
        }
    }
}
