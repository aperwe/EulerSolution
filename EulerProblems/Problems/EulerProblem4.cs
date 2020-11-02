using QBits.Intuition.Collections;
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
    [ProblemSolver("Problem 4", displayName = "Problem 4")]
    public class EulerProblem4 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Start with largest numbers to smallest.
            var left = Enumerable.Range(100, 900).Reverse().AsParallel();
            var right = Enumerable.Range(100, 900).Reverse();
            var palindromeCandidates = new SortedSet<NumericPalindrome>();
            PalindromeManipulator palindromeManipulator = new PalindromeManipulator();

            left.ForAll(a =>
            {
                //foreach (var a in left)
                //{
                foreach (var b in right)
                {
                    var candidate = new NumericPalindrome(a * b);
                    if (palindromeManipulator.IsPalindrome(candidate._numberString)) palindromeCandidates.Add(candidate);
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
