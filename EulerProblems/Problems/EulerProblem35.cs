using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The number, 197, is called a circular prime because all rotations of the digits: 197, 971, and 719, are themselves prime.
    /// There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79, and 97.
    /// How many circular primes are there below one million?
    /// </summary>
    [ProblemSolverClass("Problem 35", DisplayName = "Problem 35")]
    public class EulerProblem35 : AbstractEulerProblem
    {
        PrimeSolver primesolver;
        public EulerProblem35()
        {
            primesolver = new PrimeSolver();
        }
        protected override void Solve(out string answer)
        {
            List<int> primes = new List<int>();
            Enumerable.Range(2, 1_000_000).ToList().ForEach(number =>
                {
                    if (primesolver.IsPrime(number))
                    {
                        if (IsCircularPrime(number))
                        {
                            primes.Add(number);
                        }
                    }
                });
            answer = $"Computing... Primes {primes.Count}.";
        }
        /// <summary>
        /// Test if the given prime is a circular prime.
        /// </summary>
        /// <param name="prime">Prime</param>
        /// <returns>True if it is a circular prime.</returns>
        private bool IsCircularPrime(int prime)
        {
            var digits = prime.ToString().ToArray();
            var len = digits.Length;
            foreach (int i in Enumerable.Range(1, len))
            {
                List<char> rotation = new List<char>(digits.Skip(i));
                rotation.AddRange(digits.Take(i));
                var rotatedPrime = MoreMath.IntFromDigits(rotation);
                if (!primesolver.IsPrime(rotatedPrime)) return false;
            }
            return true;
        }
    }
}
