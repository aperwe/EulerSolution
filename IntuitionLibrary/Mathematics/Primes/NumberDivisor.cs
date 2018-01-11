using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics.Primes
{
    /// <summary>
    /// NumberDivisor contains operations related to calculating proper divisors of a number.
    /// </summary>
    public class NumberDivisor
    {
        /// <summary>
        /// Calculates the sum of proper divisors of <paramref name="number"/>.
        /// </summary>
        public long CalculateSumOfProperDivisors(long number)
        {
            PrimeSolver solver = new PrimeSolver();
            var divisorsSum = solver.GetDivisors(number).Sum();
            divisorsSum -= number; //n is not a proper divisor of itself, so remove it from the count.
            return divisorsSum;
        }

        /// <summary>
        /// Checks and returns abundancy type for a number.
        /// </summary>
        /// <param name="number">Number whose abundancy is to be checked.</param>
        /// <returns><seealso cref="AbundancyType"/> of the number.</returns>
        public AbundancyType IsAbundantOrDeficientOrPerfect(long number)
        {
            var sum = CalculateSumOfProperDivisors(number);
            var dif = sum - number;
            if (dif > 0) return AbundancyType.Abundant;
            else if (dif < 0) return AbundancyType.Deficient;
            return AbundancyType.Perfect;
        }
    }

    /// <summary>
    /// A number n is called deficient if the sum of its proper divisors is less than n and it is called abundant if this sum exceeds n.
    /// </summary>
    public enum AbundancyType
    {
        /// <summary>
        /// A number n is called deficient if the sum of its proper divisors is less than n.
        /// </summary>
        Deficient,
        /// <summary>
        /// A number n is called perfect if the sum of its proper divisors is equal to n.
        /// </summary>
        Perfect,
        /// <summary>
        /// A number n is called abundant if the sum of its proper divisors exceeds n.
        /// </summary>
        Abundant
    }
}
