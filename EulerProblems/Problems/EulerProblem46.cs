using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems
{
    /// <summary>
    /// It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.
    /// 9 = 7 + 2×12
    /// 15 = 7 + 2×22
    /// 21 = 3 + 2×32
    /// 25 = 7 + 2×32
    /// 27 = 19 + 2×22
    /// 33 = 31 + 2×12
    /// It turns out that the conjecture was false.
    /// What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?
    /// </summary>
    [ProblemSolverClass("Goldbach's other conjecture", DisplayName = "Problem 46")]
    public class EulerProblem46 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
        }
    }

}
