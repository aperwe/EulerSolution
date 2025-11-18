using QBits.Intuition.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems00to09
{
    /// <summary/>
    [ProblemSolver("Problem 4", displayName = "Problem 04", problemDefinition = 
        @"A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99.
Find the largest palindrome made from the product of two 3-digit numbers.")]
    public class EulerProblem4 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Start with largest numbers to smallest.
            var left = Enumerable.Range(100, 900).Reverse().AsParallel();
            var right = Enumerable.Range(100, 900).Reverse();
            PalindromeManipulator palindromeManipulator = new PalindromeManipulator();

            var bag = new System.Collections.Concurrent.ConcurrentBag<int>();
            left.ForAll(a =>
            {
                foreach (var b in right)
                {
                    var num = a * b;
                    if (palindromeManipulator.IsPalindrome(num.ToString()))
                        bag.Add(num);
                }
            });
            var result = bag.DefaultIfEmpty(0).Max();
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

        public int CompareTo(NumericPalindrome? right)
        {
            if (right is null) return 1;
            return this.Number.CompareTo(right.Number);
        }

        public override string ToString()
        {
            return _numberString;
        }

    }
}
