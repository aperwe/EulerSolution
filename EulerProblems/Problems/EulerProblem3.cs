using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QBits.Intuition.Mathematics.Fibonacci;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The prime factors of 13195 are 5, 7, 13 and 29. So 5*7*13*29 = 13195
    /// What is the largest prime factor of the number 600851475143 ?
    /// </summary>
    public class EulerProblem3 : AbstractEulerProblem
    {
        public override void Solve()
        {
            #region Setting up initial variables
            long testedNumber = 600851475143;
            long maxSolution = testedNumber - 1;
            long minSolution = 2;
            List<long> listOfSolutions = new List<long>();
            var upperBound = (long)Math.Sqrt(testedNumber) + 1;
            #endregion

            var currentSolution = minSolution;
            PrimeSolver primes = new PrimeSolver();

            while (currentSolution < upperBound)
            {
                if (primes.IsDivisor(testedNumber, currentSolution))
                {
                    listOfSolutions.Add(currentSolution);
                }
                currentSolution++;
            }

            var allPrimes = listOfSolutions.Where(t => primes.IsPrime(t)).ToList();
            allPrimes.Sort();
            var answer = allPrimes.Last();
            Answer = answer.ToString();

        }
    }
}
