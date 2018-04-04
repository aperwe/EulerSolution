using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /// <summary>
    /// Pandigit finder class. Use it to determine whether a given number is a pandigit number.
    /// </summary>
    public class Pandigits
    {
        /// <summary>All digits of pandigital realm.</summary>
        //private static char[] digits = { '1', '2', '3', '4', '5', '6', '7' };
        private char[] digits;
        /// <summary>
        /// Initializes the pandigit finder with specified range of digits (allowed range 0-9).
        /// </summary>
        /// <param name="start">Starting digit to include.</param>
        /// <param name="count">Count of consecutive digits to include.</param>
        public Pandigits(char start, int count)
        {
            if (start < '0') throw new ArgumentOutOfRangeException("start", "must be '0' or greater (0-9)");
            if (count > 10) throw new ArgumentOutOfRangeException("count", "digit count cannot be greater than 10 (0-9)");
            List<char> digitList = new List<char>();
            char currentChar = start;
            foreach(int num in Enumerable.Range(0, count))
            {
                digitList.Add(currentChar);
                currentChar++;
            }
            digits = digitList.ToArray();
        }
        /// <summary>Returns true if the specified number is pandigital. False otherwise.</summary>
        /// <returns>True if pandigital. False if some numbers repeat</returns>
        public bool IsPandigital(long number)
        {
            var representation = new StringBuilder().Append(number);
            return IsPandigital(representation.ToString().ToArray());
        }
        /// <summary>Returns true if the specified number is pandigital. False otherwise.</summary>
        /// <returns>True if pandigital. False if some numbers repeat</returns>
        public bool IsPandigital(NumberRepresentation numRep)
        {
            return IsPandigital(numRep.Chars);
        }
        /// <summary>Returns true if the specified number is pandigital. False otherwise.</summary>
        /// <returns>True if pandigital. False if some numbers repeat</returns>
        public bool IsPandigital(char[] chars)
        {
            return digits.All(c => chars.Contains(c));
        }
    }
}
