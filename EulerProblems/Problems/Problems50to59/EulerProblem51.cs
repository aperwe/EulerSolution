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
    [ProblemSolver("<Under construction>", "Problem 51 [TODO]",
@"By replacing the 1st digit of the 2-digit number *3, it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.

By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit number is the first example having seven primes among the ten generated numbers, yielding the family: 56003, 56113, 56333, 56443, 56663, 56773, and 56993. Consequently 56003, being the first member of this family, is the smallest prime with this property.

Find the smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, is part of an eight prime value family.>")]
    public class EulerProblem51 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new();
        protected override void Solve(out string answer)
        {
            int result1 = VerifyStep1();
            UpdateProgress($"Step 1 complete. Result: {result1}. Expected: 6");
            int result2 = VerifyStep2("56**3");
            UpdateProgress($"Step 2 complete. Result: {result2}. Expected: <TODO>");
            answer = $"Step 1 complete. Result: {result1}. Expected: 6";
        }

        /// <summary>
        /// By replacing the 1st digit of the 2-digit number *3, it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private int VerifyStep1()
        {
            // Step 1: Identify all 2-digit numbers ending with 3
            var numLength = 2; // 2-digit number
            var fixedDigit = 3; // ends with 3
            var primesFound = new List<long>();
            for (int digit = 1; digit <= 9; digit++) // first digit cannot be 0
            {
                var candidate = digit * 10 + fixedDigit;
                if (primeSolver.IsPrime(candidate))
                {
                    primesFound.Add(candidate);
                }
            }
            return primesFound.Count;
        }
        private int VerifyStep2(string primeFormat)
        {
            throw new NotImplementedException();
        }

    }
}