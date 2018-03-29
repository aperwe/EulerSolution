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
    /// We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once.
    /// For example, 2143 is a 4-digit pandigital and is also prime.
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
            var pandigitizer = new Pandigits();
            //var range = Enumerable64.Range(9_000_000_000, 0_876_543_210);
            ///var largestToSmallestRange = range.OrderByDescending<long>(0);
            //Parallelization.GetParallelRanges(7_000_000_000, 2_876_543_210, 200).ForAll(sequence =>
            Parallelization.GetParallelRanges(6_000_000_000, 1_000_000_000, 200).ForAll(sequence =>
            //var p = Enumerable64.Range(9_800_000_000, 0_076_543_210).Reverse();
            {
                foreach (var k in sequence.Reverse()) // Go from largest to smallest
                {
                    var rep = new PrimeRep(primeSolver, k);
                    if (rep.IsPrime)
                    {
                        if (pandigitizer.IsPandigital(rep.Value))
                        {
                            lock (this) list.Add(rep);
                            //break; //Because this is the largest by definition, no need to search other values.
                        }
                    }
                }
            });
            var maxItem = list.OrderByDescending(x => x.Value);
            var value = maxItem.FirstOrDefault().Value;

            answer = $"Largest n-digit pandigital prime is from 7,000,000,000 to 9,8: {value}";
        }
    }
    /// <summary>
    /// Prime representation (Facade) for a given number
    /// </summary>
    internal class PrimeRep
    {
        private readonly PrimeSolver primeSolver;
        private long Number;

        public PrimeRep(PrimeSolver primeSolver, long num)
        {
            this.primeSolver = primeSolver;
            Number = num;
        }

        public bool IsPrime => primeSolver.IsPrime(Number);
        public long Value => Number;
    }
}
