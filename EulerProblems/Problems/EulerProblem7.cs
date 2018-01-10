using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// https://projecteuler.net/problem=7
    /// By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.
    /// What is the 10 001st prime number?
    /// </summary>
    public class EulerProblem7 : AbstractEulerProblem
    {
        public override void Solve()
        {
            PrimeSolver primeSolver = new PrimeSolver();
            long value = 2;
            long whichPrime = 1; //2 is the 1st prime according to euler

            while (whichPrime < 10001)
            {
                while (!primeSolver.IsPrime(++value)) ;
                whichPrime++;
            }
            Answer = string.Format("Checked {0}th answer. The answer is: {1}", whichPrime, value);
        }
    }
}
