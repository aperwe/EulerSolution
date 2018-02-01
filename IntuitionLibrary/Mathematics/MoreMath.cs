using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /// <summary>
    /// Common mathematics operations not found in System.Math.
    /// </summary>
    public static class MoreMath
    {
        /// <summary>
        /// Calculates largest common divisor of the two values.
        /// <para/>Both values have to be positive integers.
        /// </summary>
        /// <typeparam name="T">Can be int, long</typeparam>
        /// <param name="value1">First of the values to find divisor for</param>
        /// <param name="value2">Second of the values to find divisor for</param>
        /// <returns>Largest common denominator</returns>
        /// 
        [Obsolete("Currently implementation not working. Do not use.")]
        public static T NWD<T>(T value1, T value2)
        {
            T retVal = default(T);
            if (value1.Equals(default(T))) return retVal;
            if (value2.Equals(default(T))) return retVal;

            //Now we have performed the checks
            return retVal;
        }
        /// <summary>
        /// Calculates largest common divisor of the two values.
        /// <para/>Both values have to be positive integers.
        /// </summary>
        /// <param name="value1">First of the values to find divisor for</param>
        /// <param name="value2">Second of the values to find divisor for</param>
        /// <returns>Largest common denominator</returns>
        public static int NWD(int value1, int value2)
        {
            if (value1 == 0) return 0;
            if (value2 == 0) return 0;
            if (value1 == value2) return value1; //If they are equal, the gratest common divisor is this very number.
            int greater = value2 > value1 ? value2 : value1; //Choose the greater and smaller numbers.
            int smaller = value2 < value1 ? value2 : value1;
            int remainder;
            int quotient;
            do
            {
                quotient = Math.DivRem(greater, smaller, out remainder);
                greater = smaller; //Replace the numbers with the smaller and remainder;
                smaller = remainder;
            } while (remainder != 0);
            return greater;
        }
        /// <summary>
        /// Calculates largest common divisor of the two values.
        /// <para/>Both values have to be positive integers.
        /// </summary>
        /// <param name="value1">First of the values to find divisor for</param>
        /// <param name="value2">Second of the values to find divisor for</param>
        /// <returns>Largest common denominator</returns>
        public static long NWD(long value1, long value2)
        {
            if (value1 == 0) return 0;
            if (value2 == 0) return 0;
            if (value1 == value2) return value1; //If they are equal, the gratest common divisor is this very number.
            long greater = value2 > value1 ? value2 : value1; //Choose the greater and smaller numbers.
            long smaller = value2 < value1 ? value2 : value1;
            long remainder;
            long quotient;
            do
            {
                quotient = Math.DivRem(greater, smaller, out remainder);
                greater = smaller; //Replace the numbers with the smaller and remainder;
                smaller = remainder;
            } while (remainder != 0);
            return greater;
        }

        /// <summary>
        /// If the value is too close to 0, returns 0. Otherwise the same value is returned.
        /// </summary>
        /// <param name="nearity">Margin value. Smaller values are interpreted as 0.</param>
        /// <param name="roundedOffValue">Value to round-off.</param>
        public static double RoundoffNearZero(double roundedOffValue, double nearity = 0.01)
        {
            if (Math.Abs(roundedOffValue) < nearity) return 0;
            return roundedOffValue;
        }

        /// <summary>
        /// Returns true if the specified input param is even. False otherwise.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True/False</returns>
        public static bool IsEven(Int64 value)
        {
            return (value % 2) == 0;
        }
        /// <summary>
        /// Produces a factorial for the specified number.
        /// </summary>
        /// <param name="number">Number to calculate factorial of.</param>
        /// <returns>Factorial</returns>
        public static int Factorial(int number)
        {
            //Axioms
            if (number < 0) return 0;
            if (number == 0) return 1;
            if (number == 1) return 1;
            //Otherwise iteratively calculate the factorial
            int retVal = 1;
            while (number > 1) retVal *= number--;
            return retVal;
        }
        /// <summary>
        /// Returns a number constructed from array of digits representing that int.
        /// </summary>
        /// <param name="digits">Array of digits</param>
        /// <returns>Int value</returns>
        public static int IntFromDigits(IEnumerable<int> digits)
        {
            int tenM = 1, retVal = 0;
            foreach (var d in digits)
            {
                retVal += tenM * d;
                tenM *= 10;
            }
            return retVal;
        }
        /// <summary>
        /// Returns a number constructed from array of digit chars representing that int.
        /// </summary>
        /// <param name="digits">Array of digit chars. Note, '0' = 48, '1' = 49, '2' = 50, ...</param>
        /// <returns>Int value</returns>
        public static int IntFromDigits(IEnumerable<char> digits)
        {
            int tenM = 1, retVal = 0;
            foreach (var d in digits)
            {
                retVal += tenM * (d -'0');
                tenM *= 10;
            }
            return retVal;
        }
    }
}
