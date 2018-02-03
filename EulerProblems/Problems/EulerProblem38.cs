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
    [ProblemSolverClass("Problem 38", DisplayName = "Problem 38")]
    public class EulerProblem38 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            List<Representation> list = new List<Representation>();
            Parallelization.GetParallelRanges(1, 1_000_000, 1).ForAll(sequence =>
             {
                 foreach (int number in sequence)
                 {
                     foreach (int maxMultiplier in Enumerable.Range(1, 999))
                     {
                         var candidate = new Representation(number, maxMultiplier);
                         lock (this) list.Add(candidate);
                     }
                 }

             });
            answer = $"Computing... Count = {list.Count}";
        }
        /// <summary>
        /// Model representing a number and its pandigital representation
        /// </summary>
        internal class Representation
        {
            private int Number;
            private int maxMultiplier;

            public Representation(int number, int maxMultiplier)
            {
                this.Number = number;
                this.maxMultiplier = maxMultiplier;
            }
        }
    }
}
