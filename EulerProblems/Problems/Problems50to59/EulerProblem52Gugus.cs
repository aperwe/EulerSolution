using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.Problems50to59
{
    /// <summary/>
    [ProblemSolver("<Excercise with Gugus>", "Finding primes [TODO]",
@"We will find primes smaller than 10000.")]
    public class EulerProblem52Gugus : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new();
        protected override void Solve(out string answer)
        {
            //First, we need to create an instance of a class that will help us indenty whether a specific number is a prime or not.
            //Then we will iterate from 1 to the upper bound (100) number by number and select only those that are primes.
            answer = "Start" + Environment.NewLine;

            var rangeToCheck = Enumerable64.Range(1, 10000);
            var foundPrimes = new ConcurrentBag<long>();

            foreach (var numberToCheck in rangeToCheck)
            {
                if (primeSolver.IsPrime(numberToCheck))
                {
                    foundPrimes.Add(numberToCheck);
                    answer += $"Next prime found: {numberToCheck}" + Environment.NewLine;
                }
            }

            answer += $"Found total of {foundPrimes.Count()} primes.";

        }



    }
}