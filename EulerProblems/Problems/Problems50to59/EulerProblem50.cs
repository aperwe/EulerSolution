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
    [ProblemSolver("Consecutive prime sum", "Problem 50[*]",
@"The prime 41, can be written as the sum of six consecutive primes:

                   41 = 2 + 3 + 5 + 7 + 11 + 13

This is the longest sum of consecutive primes that adds to a prime below one-hundred.

The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953.

Which prime, below one-million, can be written as the sum of the most consecutive primes?")]
    public class EulerProblem50 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new PrimeSolver();
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";

            //Step 1: Make list of all primes < 1M
            var upperBound1M = 1000 * 1000;
            //var upperBound1M = 100;
            long progress = 0; long updateCycle = 17;
            long longestLength = 0;

            IEnumerable<long> allPrimesBelow1M = primeSolver.GetPrimesSmallerThan(upperBound1M);
            var LongSummedPrimes = new List<PrimeRep>();

            //Step 2: Of them, find sums of all consecutives
            //foreach (var lowerBound in allPrimesBelow1M)
            Parallel.ForEach(allPrimesBelow1M, lowerBound =>
            {
                if (lowerBound * longestLength > upperBound1M) return; //Don't spend time if lowerbound is such that guarantees shorter result than the longest already found.
                foreach (var upperBound in allPrimesBelow1M.Reverse())
                {
                    if (lowerBound >= upperBound) continue;
                    var summedConsecutives = new PrimeRep();
                    foreach (var prime in allPrimesBelow1M.Where(x => x >= lowerBound & x <= upperBound).Select(x => x)) //150% faster than SkipWhile/TakeWhile
                    //foreach (var prime in allPrimesBelow1M.SkipWhile(x => x < lowerBound).TakeWhile(y => y <= upperBound))
                    {
                        summedConsecutives.Add(prime);
                        if (summedConsecutives.Sum > upperBound1M) break;
                    }
                    //Step 3: Find which ones sum up to a prime
                    if (primeSolver.IsPrime(summedConsecutives.Sum) & summedConsecutives.Sum < upperBound1M)
                        lock (this)
                            LongSummedPrimes.Add(summedConsecutives);
                    if (primeSolver.IsPrime(summedConsecutives.Sum) & summedConsecutives.Sum < upperBound1M) //If we found a prime sum under 1M, no need to search shorter sequences.
                        break;
                }
                //Clear the list of elements whose sequences are short
                lock (this)
                {
                    var sorted = LongSummedPrimes.OrderByDescending(x => x.Elements);
                    var longest = sorted.FirstOrDefault();
                    if (longest != null)
                    {
                        longestLength = longest.Elements;
                        LongSummedPrimes = sorted.Where(x => x.Elements >= longest.Elements).Select(x => x).ToList();
                    }
                }
                progress++;
                if ((progress % updateCycle) == 0)
                this.UpdateProgress($"Elapsed: {ElapsedTime}, Lowerbound {lowerBound}. Progress: {progress} ({progress * 100 / upperBound1M}%). Best: {LongSummedPrimes.FirstOrDefault().Elements} elements. Summed prime {LongSummedPrimes.FirstOrDefault().Sum}");
            });

            //Step 4: Identify such sequence that is the longest
            var theLongest = LongSummedPrimes.OrderByDescending(x => x.Elements).FirstOrDefault();
            answer = $"The prime, which is the sum of the most consecutive primes under 1 million is: {theLongest.Sum}. Number of elements: {theLongest.Elements}.";
        }
        /// <summary>
        /// Prime representation (Facade) for a given number
        /// </summary>
        internal class PrimeRep
        {
            private long SummedPrimes;
            private long SummedCount;

            public PrimeRep()
            {
                SummedPrimes = 0;
                SummedCount = 0;
            }

            public override string ToString()
            {
                return $"Sum={SummedPrimes} (Elements: {Elements})";
            }

            internal void Add(long prime)
            {
                SummedPrimes += prime;
                SummedCount++;
            }
            /// <summary>Returns the sum of summed primes</summary>
            internal long Sum => SummedPrimes;
            /// <summary>Returns the number of summed primes</summary>
            internal long Elements => SummedCount;
        }
    }
}