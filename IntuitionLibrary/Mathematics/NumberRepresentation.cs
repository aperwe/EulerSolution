using System;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /// <summary>
    /// Wrapper class of a long number that can be used with <seealso cref="Pandigits"/>.
    /// </summary>
    public class NumberRepresentation
    {
        private readonly long value;
        private char[] _chars;
        /// <summary>Creates an instance with specified number value.</summary>
        /// <param name="value">Number value to be represented.</param>
        public NumberRepresentation(long value)
        {
            this.value = value;
        }
        /// <summary>Readonly char array representation of the number. Fast, as this array is created only once, when first used.</summary>
        public char[] Chars
        {
            get
            {
                lock (this)
                {
                    if (_chars == null)
                    {
                        var representation = new StringBuilder().Append(value);
                        _chars = representation.ToString().ToCharArray();
                    }
                }
                return _chars;
            }
        }
        /// <summary>Gets the value represented by this wrapper.</summary>
        public long Value => value;
        /// <summary>Returns string representation of the number.</summary>
        public override string ToString()
        {
            return $"{value}";
        }
        /// <summary>
        /// Checks if the number has this specific property:
        /// Let d1 be the 1st digit, d2 be the 2nd digit, and so on. In this way, we note the following:
        /// d2d3d4=406 is divisible by 2
        /// d3d4d5=063 is divisible by 3
        /// d4d5d6=635 is divisible by 5
        /// d5d6d7=357 is divisible by 7
        /// d6d7d8=572 is divisible by 11
        /// d7d8d9=728 is divisible by 13
        /// d8d9d10=289 is divisible by 17
        /// </summary>
        /// <returns>True if the property exists. False otherwise.</returns>
        public bool IsSubstringDivisible()
        {
            var copy = Chars;
            var val2 = Take(copy, 2); if (val2 % 2 != 0) return false;
            var val3 = Take(copy, 3); if (val3 % 3 != 0) return false;
            var val4 = Take(copy, 4); if (val4 % 5 != 0) return false;
            var val5 = Take(copy, 5); if (val5 % 7 != 0) return false;
            var val6 = Take(copy, 6); if (val6 % 11 != 0) return false;
            var val7 = Take(copy, 7); if (val7 % 13 != 0) return false;
            var val8 = Take(copy, 8); if (val8 % 17 != 0) return false;
            return true;
        }

        private int Take(char[] copy, int start)
        {
            start--;
            int retVal = 0;
            retVal += (copy[start + 0] - '0') * 100;
            retVal += (copy[start + 1] - '0') * 10;
            retVal += (copy[start + 2] - '0');
            return retVal;
        }
    }
}