using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Collections
{
    /// <summary>
    /// Class that can be used to search for palindromes in a sequence of items.
    /// </summary>
    public class PalindromeManipulator
    {
        /// <summary>
        /// Simple search if the specified <paramref name="palindromeCandidate"/> is a palindrome.
        /// </summary>
        /// <param name="palindromeCandidate">String to check</param>
        /// <returns>True if sequence is a palindrome. False otherwise.</returns>
        public bool IsPalindrome(string palindromeCandidate)
        {
                if (string.IsNullOrWhiteSpace(palindromeCandidate)) return true;
                var maxLen = (palindromeCandidate.Length / 2) + 1; //+1 to cover for middle digit in numbers with odd-number of digits.
                var iterators = Enumerable.Range(0, maxLen);
                var reverseSequence = palindromeCandidate.Reverse().ToList();
                var returnValue = iterators.All(i => palindromeCandidate[i] == reverseSequence[i]); //Palindrome is when all digits from either direction match.
                return returnValue;
        }
    }
}
