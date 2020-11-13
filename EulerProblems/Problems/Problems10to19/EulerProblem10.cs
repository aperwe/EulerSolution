using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems10to19
{
    /// <summary/>
    [ProblemSolver("Problem 10", "Problem 10", problemDefinition =
@"The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
Find the sum of all the primes below two million.")]
    public class EulerProblem10 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            PrimeSolver prime = new PrimeSolver();
            long border = 2000000;
            long sumOfPrimes = 0;

            Enumerable.Range(0, (int)border).AsParallel().ForAll(a => prime.IsPrime(a));
            sumOfPrimes = prime.KnownPrimes.Aggregate((a, b) => a + b);

            answer = string.Format("The sum of all primes below {0} is equal to {1}.", border, sumOfPrimes);
        }
    }
}
