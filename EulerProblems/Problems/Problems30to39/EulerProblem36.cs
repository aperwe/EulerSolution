using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems30to39
{
    /// <summary/>
    [ProblemSolver("Double-base palindromes", displayName = "Problem 36", problemDefinition =
@"The decimal number, 585 = 1001001001^^2 (binary), is palindromic in both bases.

Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.

(Please note that the palindromic number, in either base, may not include leading zeros.)"
        )]
    public class EulerProblem36 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            PalindromeManipulator manipulator = new PalindromeManipulator();
            List<NumberRepresentation> list = new List<NumberRepresentation>();
            int counter = 0;
            int sum = 0;

            //Create Parallel stuff
            var parallels = Parallelization.GetParallelRanges(1, 999_999, 32);

            //var ranges = Enumerable.Range(1, 999_999).AsParallel();
            //foreach (var i in range)
            //ranges.ForAll(i =>
            parallels.ForAll(intrange =>
            {
                foreach (int i in intrange)
                {
                    if (i < 1_000_000)
                    {
                        NumberRepresentation number = new NumberRepresentation(i);
                        if (number.IsPalindrome)
                        {
                            lock (this)
                            {
                                list.Add(number);
                                counter++;
                                sum += number.Value;
                            }
                        }
                    }
                }
            }
            );

            answer = $"Computing... Sum = {sum}. Count = {counter}.";
        }

        /// <summary>
        /// Helper class for easy manipulation of palindromic representation of numbers.
        /// </summary>
        class NumberRepresentation
        {
            private BigInteger Number;
            private PalindromeManipulator manipulator;

            public NumberRepresentation(int value)
            {
                Number = value;
                ConvertToStrings();
                manipulator = new PalindromeManipulator();
            }

            public string DecimalString { get; private set; }
            public string BinaryString { get; private set; }
            public bool IsPalindrome => manipulator.IsPalindrome(DecimalString) && manipulator.IsPalindrome(BinaryString);
            public int Value => Number.IntValue();

            private void ConvertToStrings()
            {
                DecimalString = Number.ToString(10);
                BinaryString = Number.ToString(2);
            }
            public override string ToString()
            {
                return $"{DecimalString} - {BinaryString}";
            }
        }
    }
}
