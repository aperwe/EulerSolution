using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
    /// Find the sum of all the primes below two million.
    /// </summary>
    public class EulerProblem10 : AbstractEulerProblem
    {
        public override void Solve()
        {
            PrimeSolver prime = new PrimeSolver();
            long border = 2000000;
            long sumOfPrimes = 0;

            DateTime start = DateTime.Now;

            Enumerable.Range(0, (int)border).AsParallel().ForAll(a => prime.IsPrime(a));
            sumOfPrimes = prime.KnownPrimes.Aggregate((a, b) => a + b);

            var elapsedTime = DateTime.Now - start;

            Answer = string.Format("The sum of all primes below {0} is equal to {1}. Computation took: {2}", border, sumOfPrimes, elapsedTime);
        }
    }
}
