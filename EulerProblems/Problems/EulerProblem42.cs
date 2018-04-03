using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The n-th term of the sequence of triangle numbers is given by tn = ½n(n+1); so the first ten triangle numbers are:
    /// 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, ...
    /// 
    /// By converting each letter in a word to a number corresponding to its alphabetical position and adding these values we form a word value.For example, the word value for SKY is 19 + 11 + 25 = 55 = t10.If the word value is a triangle number then we shall call the word a triangle word.
    /// Using words.txt (right click and 'Save Link/Target As...'), 
    /// a 16K text file containing nearly two-thousand common English words, how many are triangle words?
    /// </summary>
    [ProblemSolverClass("Pandigital prime", DisplayName = "Problem 42")]
    public class EulerProblem42 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            TriangleNumbers triangleNumbers = new TriangleNumbers();
            triangleNumbers.Initialize();
            var series = new WordRepresentation(@"ProblemData\p042_words.txt");
            series.ReadDataFile();
            int countOfTriangleWords = 0;
            foreach (string word in series.words)
            {
                var number = series.Convert(word);
                if (triangleNumbers.IsTriangleNumber(number))
                {
                    countOfTriangleWords++;
                }
            }
            answer = $"Number of triangle words in file: {countOfTriangleWords}.";
        }
    }

    internal class WordRepresentation
    {
        private string fileName;
        public string[] words { get; private set; }

        public WordRepresentation(string fileName)
        {
            this.fileName = fileName;
        }


        internal void ReadDataFile()
        {
            var data = File.OpenText(fileName);
            var wordString = data.ReadToEnd();
            data.Close();

            //Extract words into an array of strings
            words = wordString.Split(',').Select(t => t.Trim('"')).ToArray();
        }

        /// <summary>
        /// Converts given word to its alphabetical representation.
        /// </summary>
        /// <param name="word">Word must be uppercase</param>
        /// <returns>Numeric representation of a word.</returns>
        internal int Convert(string word)
        {
            return word.ToArray().Select(c => c - 'A' + 1).Aggregate((sum, c) => sum + c);
        }
    }

    internal class TriangleNumbers
    {
        HashSet<int> TriangleNumbersSet = new HashSet<int>();

        internal void Initialize()
        {
            CreateSequence(TriangleNumbersSet, 100);
        }

        internal bool IsTriangleNumber(int number)
        {
            return TriangleNumbersSet.Contains(number);
        }

        /// <summary>
        /// Generates a sequence of <paramref name="count"/> triangle numbers and adds them to the <paramref name="resultingSet"/> set.
        /// </summary>
        /// <param name="resultingSet">This set will contain the specified number of triangle numbers.</param>
        /// <param name="count">Count of triangle numbers to generate.</param>
        private void CreateSequence(HashSet<int> resultingSet, int count)
        {
            foreach (int value in Enumerable.Range(1,count))
            {
                var nextTriangleNumber = (value * (value + 1)) / 2;
                resultingSet.Add(nextTriangleNumber);
            }
        }
    }
}
