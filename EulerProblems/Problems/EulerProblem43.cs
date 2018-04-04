using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// The number, 1406357289, is a 0 to 9 pandigital number because it is made up of each of the digits 0 to 9 in some order, but it also has a rather interesting sub-string divisibility property.
    /// Let d1 be the 1st digit, d2 be the 2nd digit, and so on.In this way, we note the following:
    /// d2d3d4= 406 is divisible by 2
    /// d3d4d5= 063 is divisible by 3
    /// d4d5d6= 635 is divisible by 5
    /// d5d6d7= 357 is divisible by 7
    /// d6d7d8= 572 is divisible by 11
    /// d7d8d9= 728 is divisible by 13
    /// d8d9d10= 289 is divisible by 17
    /// Find the sum of all 0 to 9 pandigital numbers with this property.
    /// </summary>
    [ProblemSolverClass("Pandigital prime", DisplayName = "Problem 43")]
    public class EulerProblem43 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            Pandigits pandigitizer = new Pandigits('0', 10);
            List<NumberRepresentation> pandigitNumbersWithProperty = new List<NumberRepresentation>();

            #region Fast - permutation based approach
            Permutations<char> permutations = new Permutations<char>();
            var list = permutations.GeneratePermutations(new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            list.AsParallel().ForAll(item =>
            {
                var numRep = new NumberRepresentation(MoreMath.IntFromDigits(item));
                if (pandigitizer.IsPandigital(numRep))
                {
                    if (numRep.IsSubstringDivisible())
                    {
                        lock (pandigitNumbersWithProperty)
                        {
                            pandigitNumbersWithProperty.Add(numRep);
                        }
                    }
                }
            }
            );
            #endregion

            #region Slow - brute force approach
            //long start = 0_000_000_000;
            //long count = 9_999_999_999;
            //long progressDone = 0;
            //Parallelization.GetParallelRanges(start, count, 200).ForAll(sequence =>
            //{
            //    var repSequence = sequence.Select(v => new NumberRepresentation(v));
            //    foreach (var rep in repSequence)
            //    {
            //        if (pandigitizer.IsPandigital(rep))
            //        {
            //            if (rep.IsSubstringDivisible())
            //            {
            //                lock (pandigitNumbersWithProperty)
            //                {
            //                    pandigitNumbersWithProperty.Add(rep);
            //                }
            //            }
            //        }
            //        #region Update progress
            //        lock (this)
            //        {
            //            progressDone++;
            //            if (progressDone % 1_000_000 == 0)
            //            {
            //                var percent = progressDone * 100.0 / count;
            //                UpdateProgress($"Range {start}-{start + count}: Done {percent}%. Hits: {pandigitNumbersWithProperty.Count}...");
            //            }
            //        }
            //        #endregion
            //    }
            //});
            #endregion
            var sum = pandigitNumbersWithProperty.Select(rep => rep.Value).Aggregate((total, num) => total + num);
            answer = $"Count = {pandigitNumbersWithProperty.Count}, Sum = {sum}";
        }
    }
}
