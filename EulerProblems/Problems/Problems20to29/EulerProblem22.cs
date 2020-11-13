using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems20to29
{
    /// <summary/>
    [ProblemSolver("Names scores", displayName = "Problem 22", problemDefinition =
@"Using names.txt (right click and 'Save Link/Target As...'), a 46K text file containing over five-thousand first names, begin by sorting it into alphabetical order. Then working out the alphabetical value for each name, multiply this value by its alphabetical position in the list to obtain a name score.

For example, when the list is sorted into alphabetical order, COLIN, which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the list. So, COLIN would obtain a score of 938 × 53 = 49714.

What is the total of all the name scores in the file?"
)]
    public class EulerProblem22 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            string dataFile = "ProblemData\\p022_names.txt";
            var data = File.OpenText(dataFile);
            var namesString = data.ReadToEnd();
            data.Close();

            //Extract names into an array of strings
            var names = namesString.Split(',').Select(t => new NameObject { Name = t.Trim('"') }).ToArray();

            var sortedNames = names.OrderBy(t => t.Name).ToArray();

            //Short test of alphabetical value.
            var test = new NameObject { Name = "COLIN" };
            var testValue = test.AlphabeticalValue;

            //Calculate the sum of all scores.
            var totalValue = 0;
            var currentPos = 1;
            foreach (var name in sortedNames)
            {
                totalValue += name.AlphabeticalValue * currentPos;
                currentPos++;
            }

            answer = string.Format("Total sum of all the name scores in the file is {0}.", totalValue);
        }


    }

    internal class NameObject
    {
        internal string Name { set; get; }
        internal int AlphabeticalValue
        {
            get
            {
                if (!_hasAlphabeticalValue)
                {
                    _internalAlphabeticalValue = this.GetAlphabeticalValue();
                    _hasAlphabeticalValue = true;
                }
                return _internalAlphabeticalValue;
            }
        }
        private int _internalAlphabeticalValue { get; set; }
        private bool _hasAlphabeticalValue { get; set; }


    }

    public static class Helper
    {
        internal static int GetAlphabeticalValue(this NameObject me)
        {
            var value = me.Name.Select(c => c - 'A' + 1).Sum();
            return value;
        }
    }
}
