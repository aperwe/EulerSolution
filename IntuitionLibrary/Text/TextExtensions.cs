using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Text
{
    /// <summary>
    /// Extension method for manipulating strings.
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Returns true if the input string contains the specified string, considering case-sensitivity.
        /// </summary>
        /// <param name="me">The string that is being tested.</param>
        /// <param name="value">The string whose presence is being tested.</param>
        /// <param name="caseSensitive">If true, strings are tested case-sensitivitely. Otherwise, the default string.Contains() is used.</param>
        /// <returns>Returns a value indicating whether the specified <see cref="System.String"/> object occurs within this string.</returns>
        public static bool Contains(this string me, string value, bool caseSensitive)
        {
            if (caseSensitive) return me.Contains(value);
            return me.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
        /// <summary>
        /// Returns true if the input string starts with the specified string, considering case-sensitivity.
        /// </summary>
        /// <param name="me">The string that is being tested.</param>
        /// <param name="value">The string whose presence is being tested.</param>
        /// <param name="caseSensitive">If true, strings are tested case-sensitivitely. Otherwise, the default string.StartsWith() is used.</param>
        /// <returns>Returns a value indicating whether the specified <see cref="System.String"/> object starts this this string.</returns>
        public static bool StartsWith(this string me, string value, bool caseSensitive)
        {
            if (caseSensitive) return me.StartsWith(value);
            return me.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Returns true if the input string ends with the specified string, considering case-sensitivity.
        /// </summary>
        /// <param name="me">The string that is being tested.</param>
        /// <param name="value">The string whose presence is being tested.</param>
        /// <param name="caseSensitive">If true, strings are tested case-sensitivitely. Otherwise, the default string.EndsWith() is used.</param>
        /// <returns>Returns a value indicating whether the specified <see cref="System.String"/> object starts this this string.</returns>
        public static bool EndsWith(this string me, string value, bool caseSensitive)
        {
            if (caseSensitive) return me.EndsWith(value);
            return me.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Returns a copy of the string, but ensures it is not longer than the specified <paramref name="maxLength"/>.
        /// If the original string exceeds that length, it adds an elypsis sign and truncates, so that the string is not longer than the limit.
        /// </summary>
        /// <param name="me">Input string to be truncated if needed.</param>
        /// <param name="maxLength">Length limit.</param>
        /// <returns>Original or truncated original string.</returns>
        public static string MaxLength(this string me, int maxLength)
        {
            string elypsis = "(...)";
            var elypsisLength = elypsis.Length;
            if (me.Length <= maxLength) return me;

            //corner cases
            if (maxLength == 0) return string.Empty;
            if (maxLength <= elypsisLength) return me.Substring(0, maxLength); //No point in appending elypsis, because this would be the only string returned.

            //truncation needed
            return string.Format("{0}{1}", me.Substring(0, maxLength - elypsisLength), elypsis);
        }
    }
}
