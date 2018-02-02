using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The decimal number, 585 = 10010010012 (binary), is palindromic in both bases.
    /// Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.
    /// (Please note that the palindromic number, in either base, may not include leading zeros.)
    /// </summary>
    [ProblemSolverClass("Problem 36", DisplayName = "Problem 36")]
    public class EulerProblem36 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            PalindromeManipulator manipulator = new PalindromeManipulator();
            List<NumberRepresentation> list = new List<NumberRepresentation>();
            int counter = 0;
            int sum = 0;

            //Create Parallel stuff
            var parallels = GetParallelRanges(1, 999999, 32);

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
        /// Gets a list of sub-ranges that can be used in parallel loop to iterate over whole range.
        /// </summary>
        /// <param name="start">The value of the first integer in this sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <param name="partitions">The number of partitions (sub-ranges) into which to divide the whole range produced.</param>
        /// <returns>Easily parallelizable list of partitions on which you can call .ForAll() delegate.</returns>
        public ParallelQuery<IEnumerable<int>> GetParallelRanges(int start, int count, int partitions)
        {
            List<IEnumerable<int>> enumerables = new List<IEnumerable<int>>();
            int partitionSize = (count + 1) / partitions; //Handle odd counts properly
            int end = count + start;
            for (int pos = start, range = partitionSize; pos < end; pos += partitionSize)
            {
                if ((pos + range) > end) //Make sure the last partition is capped to ensure proper total count
                {
                    range = end - pos;
                }
                var item = Enumerable.Range(pos, range);
                enumerables.Add(item);
            }
            var parallels = enumerables.AsParallel();
            return parallels;
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
