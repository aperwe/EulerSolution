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
        PrimeSolver primeSolver = new PrimeSolver();
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
            int start = 3 - 2;
            bool failure = true; //Flag to continue searching
            var current = start;
            while (failure)
            {
                current++;
                var first = current;
                var firstFactors = GetAllPrimeFactors(first);
                if (firstFactors.Distinct().Count() < 4) continue;

                var second = first + 1;
                var secondFactors = GetAllPrimeFactors(second);
                if (secondFactors.Distinct().Count() < 4) continue;

                var third = second + 1;
                var thirdFactors = GetAllPrimeFactors(third);
                if (thirdFactors.Distinct().Count() < 4) continue;

                var fourth = third + 1;
                var fourthFactors = GetAllPrimeFactors(fourth);
                if (fourthFactors.Distinct().Count() < 4) continue;
                //Found a quad of four consecutive numbers with prime factors.
                UpdateProgress($"Found: ({first}).");
                answer = $"Found: ({first}).";
                failure = false;
            }

        }

        /// <summary>Gets a list of all factors of specified numbers.</summary>
        /// <param name="inputNumber"></param>
        private IEnumerable<long> GetAllPrimeFactors(long inputNumber)
        {
            var primeDivisors = new List<long>(); //The return value
            var currentlyAnalysedNumber = inputNumber;
            bool exitflag = true;

            while (exitflag)
            {
                var allFactors = primeSolver.GetDivisors(currentlyAnalysedNumber);

                var onlyPrimeFactors = from x in allFactors
                                       where primeSolver.IsPrime(x)
                                       select x;

                primeDivisors.AddRange(onlyPrimeFactors);

                var mul = primeDivisors.Aggregate((x, mult) => x * mult); //Multiply all found primes

                long temp = inputNumber / mul;
                if (temp > 1)
                {
                    currentlyAnalysedNumber = temp;
                }
                else //If remainder is 1 (or less - unlikely) break the loop;
                {
                    exitflag = false;
                }
                // || (mul == inputNumber)
            }
            return primeDivisors;
        }
    }
}