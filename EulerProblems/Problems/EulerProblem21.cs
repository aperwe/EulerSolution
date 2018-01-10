using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n).
    /// If d(a) = b and d(b) = a, where a ≠ b, then a and b are an amicable pair and each of a and b are called amicable numbers.
    /// 
    /// For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220.
    /// 
    /// Evaluate the sum of all the amicable numbers under 10000.
    /// </summary>
    public class EulerProblem21 : AbstractEulerProblem
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;

            long n = 220;
            long f = CalculateSumOfProperDivisors(n);
            Answer = f.ToString();

            Dictionary<long, long> mapOfNumbersToDivisors = new Dictionary<long, long>();

            // Identify the list of all d() functions under 10000.
            foreach (int number in Enumerable.Range(1, 10000))
            {
                long dFunction = CalculateSumOfProperDivisors(number);
                mapOfNumbersToDivisors.Add(number, dFunction);
            }
            //mapOfNumbersToDivisors.Add(220, d(220));
            //mapOfNumbersToDivisors.Add(284, d(284));

            List<long> amicableNumbers = new List<long>();

            // Find the amicable numbers
            mapOfNumbersToDivisors.AsParallel().ForAll(mapEntry =>
            {
                long key = mapEntry.Key;
                if (amicableNumbers.Contains(key)) return; // Make sure we do not count the same amicable number twice.

                long value = mapEntry.Value;

                try
                {
                    long reverseKey = mapOfNumbersToDivisors[value];
                    if (key == reverseKey) //Key and value are the two amicable numbers. Add them to the list to remember.
                    {
                        if (key != value) //amicability requires that d(a) = b and d(b) = a, where a != b
                        {
                            amicableNumbers.Add(key);
                            amicableNumbers.Add(value);
                        }
                    }

                }
                catch (KeyNotFoundException e)
                {
                    //There is no reverse value in the collection, so no chance of amicability.
                    string m = e.Message;
                }
            });
            var sumOfAmicableNumbers = amicableNumbers.Sum();

            var elapsedTime = DateTime.Now - start;
            Answer = string.Format("Elapsed computation time: {0}. The sum of all the amicable numbers under 10000 is {1}", elapsedTime, sumOfAmicableNumbers);

        }

        /// <summary>
        /// Calculates the sum of proper divisors of <paramref name="number"/>.
        /// </summary>
        private long CalculateSumOfProperDivisors(long number)
        {
            PrimeSolver solver = new PrimeSolver();
            var divisors = solver.GetDivisors(number);
            var sum = divisors.Aggregate((p, acc) => acc + p);
            sum -= number; //n is not a proper divisor of itself, so remove it from the count.
            return sum;
        }
    }
}
