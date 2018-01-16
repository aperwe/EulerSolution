using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// http://projecteuler.net/problem=4
    /// A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99.
    /// Find the largest palindrome made from the product of two 3-digit numbers.
    /// </summary>
    [ProblemSolverClass("Problem 4", DisplayName = "Problem 4")]
    public class EulerProblem4 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Start with largest numbers to smallest.
            var left = Enumerable.Range(100, 900).Reverse().AsParallel();
            var right = Enumerable.Range(100, 900).Reverse();
            var palindromeCandidates = new SortedSet<NumericPalindrome>();

            left.ForAll(a =>
            {
                //foreach (var a in left)
                //{
                foreach (var b in right)
                {
                    var candidate = new NumericPalindrome(a * b);
                    if (candidate.IsPalindrome) palindromeCandidates.Add(candidate);
                }
            });
            var result = palindromeCandidates.Last().Number;
            answer = string.Format("The largest palindrome made from the product of two 3-digit numbers is: {0}.", result);
        }
    }

    public class NumericPalindrome : IComparable<NumericPalindrome>
    {
        public string _numberString { get; set; }
        private int _number;
        public int Number
        {
            get { return _number; }
        }

        public NumericPalindrome(int number)
        {
            _number = number;
            _numberString = number.ToString();
        }

        public bool IsPalindrome
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_numberString)) return true;
                var maxLen = (_numberString.Length / 2) + 1; //+1 to cover for middle digit in numbers with odd-number of digits.
                var iterators = Enumerable.Range(0, maxLen);
                var reverseSequence = _numberString.Reverse().ToList();
                var returnValue = iterators.All(i => _numberString[i] == reverseSequence[i]); //Palindrome is when all digits from either direction match.
                return returnValue;
            }
        }

        public int CompareTo(NumericPalindrome right)
        {
            return this.Number.CompareTo(right.Number);
        }

        public override string ToString()
        {
            return _numberString;
        }

    }
}
