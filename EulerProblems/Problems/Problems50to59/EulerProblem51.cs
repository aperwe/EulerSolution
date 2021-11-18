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

namespace EulerProblems.Problems.Problems50to59
{
    /// <summary/>
    [ProblemSolver("Prime digit replacements", "Problem 51",
@"By replacing the 1st digit of the 2-digit number *3, it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.

By replacing the 3rd and 4th digits of 56**3 with the same digit - X, - this 5-digit number is the first example having seven primes among the ten generated numbers, yielding the family:
    56003, 56113, 56333, 56443, 56663, 56773, and 56993.
Consequently 56003, being the first member of this family, is the smallest prime with this property.

Find the smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, is part of an eight prime value family.")]
    public class EulerProblem51 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new PrimeSolver();
        object locker = new object();
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
            long smallestPrime = Check1DigitReplacement();

            answer += $"Finished. Smallest prime = {smallestPrime}.";
        }

        /// <summary>
        /// Checks replacement of 1 digit in primes up to 10 digits long
        /// </summary>
        private long Check1DigitReplacement()
        {
            long smallestPrime = 0;

            foreach (var l1 in Enumerable.Range(0, 10))
            {
                foreach (var l2 in Enumerable.Range(0, 10))
                {
                    foreach (var l3 in Enumerable.Range(0, 10))
                    {
                        foreach (var l4 in Enumerable.Range(0, 10))
                        {
                            long candidatePrime = CheckPrimesByChanging1stDigit(l4, l3, l2, l1);
                            if (candidatePrime != 0)
                            {
                                if (smallestPrime == 0) smallestPrime = candidatePrime;
                                else if (candidatePrime < smallestPrime) smallestPrime = candidatePrime;
                            }
                        }
                    }
                }
            }
            return smallestPrime;
        }

        /// <summary>
        /// Checks number of primes by adding first digit between all <paramref name="l1"/>, <paramref name="l2"/>, <paramref name="l3"/>, and <paramref name="l4"/>
        /// Number is in the form of [*] [l4] [l3] [l2] [l1]
        /// </summary>
        /// <param name="l4">1000*x</param>
        /// <param name="l3">100*x</param>
        /// <param name="l2">10*x</param>
        /// <param name="l1">1*x</param>
        /// <returns>If 8 primes are found - smallest of them. Otherwise 0.</returns>
        private long CheckPrimesByChanging1stDigit(int l4, int l3, int l2, int l1)
        {
            long seedNumber = 0, multiplier = 1; //Initial

            seedNumber += l1 * multiplier; multiplier *= 10;
            seedNumber += l2 * multiplier; multiplier *= 10;
            seedNumber += l3 * multiplier; multiplier *= 10;
            seedNumber += l4 * multiplier; multiplier *= 10;

            int howManyPrimes = 0;
            long smallestPrime = 0;
            bool smallestSet = false; //Set to true if smallest prime has been set

            foreach (var replacingDigit in Enumerable.Range(0, 10))
            {
                long testedNumber = replacingDigit * multiplier + seedNumber;
                if (primeSolver.IsPrime(testedNumber))
                {
                    howManyPrimes++;
                    if (!smallestSet)
                    {
                        smallestSet = true;
                        smallestPrime = testedNumber;
                    }
                    else
                    {
                        if (testedNumber < smallestPrime) smallestPrime = testedNumber;
                    }
                }
            }
            return howManyPrimes == 8 ? smallestPrime : 0;
        }
    }
}