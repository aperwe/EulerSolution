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
    [ProblemSolver("Prime permutations", "Problem 49",
@"The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways:
(i)  each of the three terms is a prime, and
(ii) each of the 4-digit numbers are permutations of one another

There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, but there is one other 4-digit increasing sequence.

What 12-digit number do you form by concatenating the three terms in this sequence?")]
    public class EulerProblem49 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new PrimeSolver();
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";

            //Step 1: Find all 4-digit primes
            var fourDigitPrimes = new SortedSet<PrimeRep>();
            for (int i = 1000; i < 10000; i++)
            {
                if (primeSolver.IsPrime(i))
                {
                    fourDigitPrimes.Add(new PrimeRep(primeSolver, i));
                }

            }
            answer = $"Found {fourDigitPrimes.Count} 4-digit primes. Smallest {fourDigitPrimes.First()}, largest {fourDigitPrimes.Last()}.";

            //Step 2: Find prime-triplets
            var allTriplets = new Stack<PrimeTriplet>();
            Parallel.ForEach(fourDigitPrimes, first =>
            //foreach (var first in fourDigitPrimes)
            {
                foreach (var second in fourDigitPrimes.SkipWhile(item => item.Value <= first.Value).Select(item => item)) //skip all duplicates of first
                {
                    if (first.Equals(second)) continue;
                    foreach (var third in fourDigitPrimes.SkipWhile(item => item.Value <= second.Value).Select(item => item)) //skipp all duplicates of second
                    {
                        if (second.Equals(third)) continue;

                        //Step 3: Identify a triplet that is a prime and are permutations of one another and arithmentic sequence
                        if (string.Equals(first.OrderedPermutation, second.OrderedPermutation))
                        if (string.Equals(second.OrderedPermutation, third.OrderedPermutation))
                        if (second.Value - first.Value == third.Value - second.Value)

                            lock (allTriplets)
                                allTriplets.Push(new PrimeTriplet { First = first, Second = second, Third = third });
                    }
                }
            });
            answer += $"\n{allTriplets.Count} triplets created";
            foreach(var triplet in allTriplets)
            {
                answer += $"\n- {triplet.TwelveDigitNumber},";
            }

            //Step 4: Concatenate them in growing numbers and return the result
            //answer = $"Solution: TBD'";
        }
        /// <summary>
        /// Prime representation (Facade) for a given number
        /// </summary>
        internal class PrimeRep : IComparable
        {
            private readonly PrimeSolver primeSolver;
            private long Number;
            private string NumberAsString;
            private string OrderedPermutationAsString;

            public PrimeRep(PrimeSolver primeSolver, long num)
            {
                this.primeSolver = primeSolver;
                Number = num;
            }

            public bool IsPrime => primeSolver.IsPrime(Number);
            public long Value => Number;
            public string NumberString
            {
                get
                {
                    if (string.IsNullOrEmpty(NumberAsString))
                    {
                        NumberAsString = Value.ToString();
                    }
                    return NumberAsString;
                }
            }

            public string OrderedPermutation
            {
                get
                {
                    if (string.IsNullOrEmpty(OrderedPermutationAsString))
                    {
                        OrderedPermutationAsString = new string(NumberString.OrderBy(c => c).ToArray());
                    }
                    return OrderedPermutationAsString;
                }
            }

            #region IComparable interface
            public int CompareTo(object obj)
            {
                return Number.CompareTo(((PrimeRep)obj).Number);
            }
            #endregion
            public override string ToString()
            {
                return Number.ToString();
            }
        }
        /// <summary>
        /// Triplet holding 3 primes
        /// </summary>
        private class PrimeTriplet
        {
            internal PrimeRep First;
            internal PrimeRep Second;
            internal PrimeRep Third;

            public string TwelveDigitNumber => $"{First}{Second}{Third}";

            public override string ToString()
            {
                return $"{First}, {Second}, {Third}";
            }
        }
    }
}