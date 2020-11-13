using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace EulerProblems.Problems.Problems60to69
{
    [ProblemSolver("Problem 67", displayName = "Problem 67")]
    public class EulerProblem67 : Problems10to19.EulerProblem18
    {
        protected override void Solve(out string answer)
        {
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

            answer = string.Format("Maximum total from top to bottom of the triangle is {0}.", maximum);
        }
    }
}
