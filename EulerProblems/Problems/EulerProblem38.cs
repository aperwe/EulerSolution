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
    /// Take the number 192 and multiply it by each of 1, 2, and 3:
    /// 192 × 1 = 192
    /// 192 × 2 = 384
    /// 192 × 3 = 576
    /// 
    /// By concatenating each product we get the 1 to 9 pandigital, 192384576. We will call 192384576 the concatenated product of 192 and(1,2,3)
    /// The same can be achieved by starting with 9 and multiplying by 1, 2, 3, 4, and 5, giving the pandigital, 918273645, which is the concatenated product of 9 and(1,2,3,4,5).
    /// What is the largest 1 to 9 pandigital 9-digit number that can be formed as the concatenated product of an integer with(1,2, ... , n) where n > 1?
    /// </summary>
    [ProblemSolver("Problem 38", displayName = "Problem 38")]
    public class EulerProblem38 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var list = new List<Representation>();
            Parallelization.GetParallelRanges(1, 10_000, 4).ForAll(sequence =>
             {
                 foreach (int number in sequence)
                 {
                     foreach (int maxMultiplier in Enumerable.Range(2, 99))
                     {
                         var candidate = new Representation(number, maxMultiplier);
                         if (candidate.IsPandigital())
                         {
                             lock (this) list.Add(candidate);
                             break; //If pandigit is found, multiplying by higher multipliers definitely will fail.
                         }
                     }
                 }

             });
            var result = list.OrderByDescending(item => item.Pandigits);
            var max = result.First();
            answer = $"Count = {list.Count}, Largest pandigital: {max}";
        }
        /// <summary>
        /// Model representing a number and its pandigital representation
        /// </summary>
        internal class Representation
        {
            private int Number;
            private int MaxMultiplier;
            private static char[] digits = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Data data = new Data();
            internal string Pandigits => data.Pandigit;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="number"></param>
            /// <param name="maxMultiplier">n>1</param>
            public Representation(int number, int maxMultiplier)
            {
                Number = number;
                if (maxMultiplier < 2) throw new ArgumentOutOfRangeException("maxMultiplier", "Value must be n>1");
                MaxMultiplier = maxMultiplier;
            }

            /// <summary>
            /// Returns true if the specified number, concatenated with its <see cref="MaxMultiplier"/> multiplications is pandigital. False otherwise.
            /// </summary>
            /// <returns></returns>
            internal bool IsPandigital()
            {
                StringBuilder representation = new StringBuilder().Append(Number);
                for (int multiplier = 2; multiplier <= MaxMultiplier; multiplier++)
                {
                    representation.Append(Number * multiplier);
                    if (representation.Length > 9) return false; //String larger than 9 cannot be 1-9 pandigital
                    if (IsPandigital(representation.ToString().ToArray()))
                    {
                        data.Pandigit = representation.ToString();
                        data.Number = Number;
                        data.Multiplier = multiplier;
                        return true;
                    }
                }
                return false;
            }


            private bool IsPandigital(char[] chars)
            {
                return digits.All(c => chars.Contains(c));
            }
            public override string ToString()
            {
                return $"{Number} ({data.Number}*{data.Multiplier}={data.Pandigit})";
            }
        }
        struct Data
        {
            internal string Pandigit;
            internal int Number;
            internal int Multiplier;
        }
    }
}
