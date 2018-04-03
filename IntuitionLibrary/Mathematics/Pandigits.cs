using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    public class Pandigits
    {
        /// <summary>All digits of pandigital realm.</summary>
        private static char[] digits = { '1', '2', '3', '4', '5', '6', '7' };
        /// <summary>Returns true if the specified number is pandigital. False otherwise.</summary>
        /// <returns>True if pandigital. False if some numbers repeat</returns>
        public bool IsPandigital(long number)
        {
            var representation = new StringBuilder().Append(number);
            return IsPandigital(representation.ToString().ToArray());
        }

        private bool IsPandigital(char[] chars)
        {
            return digits.All(c => chars.Contains(c));
        }
    }
}
