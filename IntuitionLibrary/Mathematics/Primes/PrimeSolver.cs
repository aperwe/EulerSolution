using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics.Primes
{
    /// <summary>
    /// Manages prime numbers management. Threadsafe.
    /// </summary>
    public class PrimeSolver
    {
        SortedSet<long> knownPrimes = new SortedSet<long>();

        /// <summary>
        /// Returns the list of all primes already known by this instance of <seealso cref="PrimeSolver"/>
        /// </summary>
        public IEnumerable<long> KnownPrimes
        {
            get
            {
                return knownPrimes.AsEnumerable();
            }
        }

        /// <returns>Count of known primes.</returns>
        public override string ToString()
        {
            return string.Format("Known primes: {0}", knownPrimes.Count);
        }

        /// <summary>
        /// Tests if a given number is a prime. If so, true is returned.
        /// Note: This operation can take a long time if the number has not been cached yet.
        /// Note: 2 is assumed to be the first prime.
        /// </summary>
        /// <param name="testedInteger"></param>
        /// <returns>True for primes. False otherwise.</returns>
        public bool IsPrime(long testedInteger)
        {
            #region Sanity checks
            if (testedInteger < 2) return false;
            #endregion

            #region Check cache
            if (knownPrimes.Contains(testedInteger)) return true;
            #endregion

            MakeTest(testedInteger);
            if (knownPrimes.Contains(testedInteger)) return true;

            return false;
        }

        /// <summary>
        /// Calculates and returns the number of divisors of <paramref name="testedInteger"/>.
        /// </summary>
        /// <param name="testedInteger">Positive number to be tested.</param>
        /// <returns>Number of divisors of <paramref name="testedInteger"/></returns>
        public int CalculateDivisors(long testedInteger)
        {
            int result = 0;
            #region Special cases
            if (testedInteger == 1) return 1; //Two divisors (1 and itself (also 1)) are identical. Do not count them twice.
            #endregion

            long maxDivisor = (long)Math.Sqrt(testedInteger);

            result = Enumerable.Range(1, (int)maxDivisor).Count(e => IsInteger(testedInteger, e));
            result *= 2; //Each division means there are 2 divisors found. Except when the divisor is a square root of the number - then it should be counted as 1, not 2 divisors.
            if (maxDivisor * maxDivisor == testedInteger) result--; //test if the number is a square of one of its divisors. If so, reduce the number of divisors by 1 duplicate.

            return result;
        }

        /// <summary>
        /// Computes the list of all divisors of the given number.
        /// </summary>
        public List<long> GetDivisors(long number)
        {
            List<long> listOfSolutions = new List<long>();
            var upperBound = (long)Math.Sqrt(number) + 1;
            long currentSolution = 1;
            long otherDivisor;

            while (currentSolution < upperBound)
            {
                if (IsDivisor(number, currentSolution, out otherDivisor))
                {
                    listOfSolutions.Add(currentSolution);
                    if (currentSolution != otherDivisor) //Do not add the same number twice (such as square of the number)
                    {
                        listOfSolutions.Add(otherDivisor);
                    }
                }
                currentSolution++;
            }
            return listOfSolutions;
        }


        /// <summary>
        /// Tests whether the <paramref name="divisor"/> is a divisor of <paramref name="number"/>.
        /// </summary>
        /// <returns>True if <param name="divisor"/> is a divisor of <param name="number"/>. False otherwise.</returns>
        public bool IsDivisor(long number, long divisor)
        {
            var division = number / divisor;
            var testedMultiple = division * divisor;
            var isADivisor = testedMultiple == number;
            return isADivisor;
        }

        /// <summary>
        /// Tests whether the <paramref name="divisor"/> is a divisor of <paramref name="number"/>.
        /// </summary>
        /// <param name="result">If <paramref name="divisor"/> is a divisor, then <paramref name="result"/> is filled with the result of the division, which is effectively the other divisor.</param>
        /// <returns>True if <param name="divisor"/> is a divisor of <param name="number"/>. False otherwise.</returns>
        public bool IsDivisor(long number, long divisor, out long result)
        {
            var division = number / divisor;
            var testedMultiple = division * divisor;
            var isADivisor = testedMultiple == number;
            result = division;
            return isADivisor;
        }

        /// <summary>
        /// Tests using this simple trial method: http://www.wikihow.com/Check-if-a-Number-Is-Prime
        /// If test is passed (i.e. the number is a prime), it is added to the set of known primes.
        /// </summary>
        /// <param name="testedInteger">Number tested for primality.</param>
        private void MakeTest(long testedInteger)
        {
            #region Special cases
            if (testedInteger == 2)
            {
                knownPrimes.Add(testedInteger);
                return;
            }
            #endregion

            #region Divide n by 2. If the result is an integer, then n is not prime because 2 is a factor of n. Look at the last digit and if it's an even number, it's divisible by 2. If not, continue.
            if (IsInteger(testedInteger, 2)) return;
            if (IsInteger(testedInteger, 4)) return; // Is this necessary?
            #endregion

            #region The brute-force test until sqrt(a)
            long maxDivisor = (long)Math.Sqrt(testedInteger);

            for (long i = 3; i <= maxDivisor; i++)
            {
                if (IsInteger(testedInteger, i)) return;
            }
            #endregion

            //Now we are pretty sure the number is a prime. Add it to the known set of cached primes.
            lock (knownPrimes)
            knownPrimes.Add(testedInteger);
        }

        /// <summary>
        /// Returns true if result of division of <paramref name="testedInteger"/> and <paramref name="divisor"/> is an integer. False otherwise.
        /// </summary>
        /// <param name="testedInteger"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        private bool IsInteger(long testedInteger, long divisor)
        {
            var b = testedInteger / divisor;
            var c = b * divisor;
            return c == testedInteger;
        }
    }
}
