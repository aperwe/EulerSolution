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
            var divisors = solver.GetDivisors(number);
            var sum = divisors.Aggregate((p, acc) => acc + p);
            sum -= number; //n is not a proper divisor of itself, so remove it from the count.
            return sum;
        }

    }
}
