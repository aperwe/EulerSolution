using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. For example, 2143 is a 4-digit pandigital and is also prime.
    /// What is the largest n-digit pandigital prime that exists?
    /// </summary>
    [ProblemSolverClass("Pandigital prime", DisplayName = "Problem 41")]
    public class EulerProblem41 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Of course we will assume that this number will start with 9
            var list = new List<PrimeRep>();
            var primeSolver = new PrimeSolver();
            Parallelization.GetParallelRanges(1, 9_876_543_210, 10).ForAll(range =>
            {
                foreach (int num in range)
                {
                    var rep = new PrimeRep(primeSolver, num);
                    if (rep.IsPrime)
                    {
                        lock (this) list.Add(rep);
                    }
                }
            }
            );
            answer = $"Computing... Count = {list.Count}";
        }
    }
    internal class PrimeRep
    {
        private readonly PrimeSolver primeSolver;
        private static char[] digits = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private int Number;

        public PrimeRep(PrimeSolver primeSolver, int num)
        {
            this.primeSolver = primeSolver;
            Number = num;
        }

        public bool IsPrime => primeSolver.IsPrime(Number);
    }
}
