using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The decimal number, 585 = 10010010012 (binary), is palindromic in both bases.
    /// Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.
    /// (Please note that the palindromic number, in either base, may not include leading zeros.)
    /// </summary>
    [ProblemSolverClass("Problem 36", DisplayName = "Problem 36")]
    public class EulerProblem36 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var range = Enumerable.Range(1, 999_999);
            var test = 7;
            answer = $"Computing... Last = {range.Max()}";
        }
    }
}
