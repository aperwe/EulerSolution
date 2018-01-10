using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace EulerProblems.Problems
{
    public class EulerProblem67 : EulerProblem18
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;

            string triangleDataFileName = "ProblemData\\p067_triangle.txt";
            var fileStream = File.OpenText(triangleDataFileName);

            List<string> linesOfText = new List<string>();
            while (!fileStream.EndOfStream)
            {
                var singleLineOfText = fileStream.ReadLine();
                linesOfText.Add(singleLineOfText);
            }
            fileStream.Close();
            var triangleData = linesOfText.ToArray();

            var maximum = FindMaximum(triangleData);

            var elapsedTime = DateTime.Now - start;
            Answer = string.Format("Elapsed computation time: {0}. Maximum total from top to bottom of the triangle is {1}.", elapsedTime, maximum);
        }
    }
}
