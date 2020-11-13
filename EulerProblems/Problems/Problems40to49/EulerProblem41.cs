using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems40to49
{
    /// <summary/>
    [ProblemSolver("Pandigital prime", displayName = "Problem 41", problemDefinition =
@"We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. For example, 2143 is a 4-digit pandigital and is also prime.

What is the largest n-digit pandigital prime that exists?"
        )]
    public class EulerProblem41 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Of course we will assume that this number will start with 9
            var list = new List<PrimeRep>();
            var primeSolver = new PrimeSolver();
            var pandigitizer = new Pandigits('1', 7);
            long start = 0_000_000;
            long count = 7_654_321;
            long progressDone = 0;
            Parallelization.GetParallelRanges(start, count, 200).ForAll(sequence =>
            {
                foreach (var num in sequence.Reverse()) // Go from largest to smallest
                {
                    if (pandigitizer.IsPandigital(num))
                    {
                        var rep = new PrimeRep(primeSolver, num);
                        if (rep.IsPrime)
                        {
                            if (pandigitizer.IsPandigital(rep.Value))
                            {
                                lock (list) list.Add(rep);
                                //break; //Because this is the largest by definition, no need to search other values.
                            }
                        }
                    }
                    #region Update progress
                    lock (this)
                    {
                        progressDone++;
                        if (progressDone % 100_000 == 0)
                        {
                            var percent = progressDone * 100.0 / count;
                            UpdateProgress($"Range {start}-{start + count}: Done {percent}%. Hits: {list.Count}...");
                        }
                    }
                    #endregion
                }
            });
            var maxItem = list.OrderByDescending(x => x.Value);
            var finalRep = maxItem.FirstOrDefault(); //If no prime found, this will be 'null'

            long value = 0; //Default
            if (finalRep != null) value = finalRep.Value; //Override if exists

            answer = $"Largest n-digit pandigital prime in range {start} - {start+count}: {value}";
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
