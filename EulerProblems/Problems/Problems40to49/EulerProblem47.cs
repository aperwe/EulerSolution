using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.Problems40to49
{
    /// <summary/>
    [ProblemSolver("Distinct primes factors", "Problem 47",
@"The first two consecutive numbers to have two distinct prime factors are:

14 = 2 × 7
15 = 3 × 5

The first three consecutive numbers to have three distinct prime factors are:

644 = 2² × 7 × 23
645 = 3 × 5 × 43
646 = 2 × 17 × 19.

Find the first four consecutive integers to have four distinct prime factors each. What is the first of these numbers?")]
    public class EulerProblem47 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
            int start = 644 - 1;
            bool failure = true; //Flag to continue searching
            var current = start;
            while (failure)
            {
                current++;
                var first = current;
                var firstFactors = GetAllFactors(first);
                if (!AllPrime(firstFactors)) continue;

                var second = first + 1;
                var secondFactors = GetAllFactors(second);
                if (!AllPrime(secondFactors)) continue;

                var third = second + 1;
                var thirdFactors = GetAllFactors(third);
                if (!AllPrime(thirdFactors)) continue;

                var fourth = third + 1;
                var fourthFactors = GetAllFactors(fourth);
                if (!AllPrime(fourthFactors)) continue;
                UpdateProgress($"Current: ({current}).");
            }

        }
        /// <summary>Gets a list of all factors of specified numbers.</summary>
        /// <param name="number"></param>
        private IEnumerable<long> GetAllFactors(long number)
        {
            QBits.Intuition.Mathematics.Primes.PrimeSolver primeSolver = new PrimeSolver();
            return primeSolver.GetDivisors(number);
        }
        /// <summary>Checks if all numbers in the list are primes</summary>
        /// <param name="firstFactors"></param>
        /// <returns></returns>
        private bool AllPrime(IEnumerable<long> thirdFactors)
        {
            throw new NotImplementedException();
        }
    }
}