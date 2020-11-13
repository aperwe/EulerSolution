using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems11to99
{
    /// <summary/>
    [ProblemSolver("Power digit sum", displayName = "Problem 16", problemDefinition =
@"2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.

What is the sum of the digits of the number 2^1000?"
        )]
    public class EulerProblem16 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            BigInteger power = 1;
            foreach(int iterator in Enumerable.Range(1, 1000))
            {
                power *= 2;
            }

            string stringOfDigits = power.ToString();
            int sum = 0;
            foreach(char c in stringOfDigits)
            {
                var digit = int.Parse(c.ToString());
                sum += digit;
            }
            answer = string.Format("2^1000 = {0}. Sum of digits = {1}.", power, sum);
        }
    }
}
