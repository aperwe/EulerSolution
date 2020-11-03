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
    /// <summary/>
    [ProblemSolver("Goldbach's other conjecture", "Problem 46",
@"It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.
 9 = 7 + 2×1^2
 15 = 7 + 2×2^2
 21 = 3 + 2×3^2
 25 = 7 + 2×3^2
 27 = 19 + 2×2^2
 33 = 31 + 2×1^2
It turns out that the conjecture was false.
What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?")]
    public class EulerProblem46 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
        }
    }

}
