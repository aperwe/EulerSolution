using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// A perfect number is a number for which the sum of its proper divisors is exactly equal to the number.
    /// For example, the sum of the proper divisors of 28 would be 1 + 2 + 4 + 7 + 14 = 28, which means that 28 is a perfect number.
    /// A number n is called deficient if the sum of its proper divisors is less than n and it is called abundant if this sum exceeds n.
    /// As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the smallest number that can be written as the sum of two abundant numbers is 24.
    /// By mathematical analysis, it can be shown that all integers greater than 28123 can be written as the sum of two abundant numbers. However, this upper limit
    /// cannot be reduced any further by analysis even though it is known that the greatest number that cannot be expressed as the sum of two abundant numbers is less
    /// than this limit.
    /// 
    /// Find the sum of all the positive integers which cannot be written as the sum of two abundant numbers.
    /// </summary>
    [ProblemSolverClass("Problem 23", DisplayName = "Problem 23")]
    public class EulerProblem23 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var testedRange = Enumerable.Range(1, 28123).AsParallel();

            long summator = 0;
            //Parallel.ForEach()
            testedRange.ForAll(number =>
            {
                NumberDivisor numberDivisor = new NumberDivisor();
                bool hasBeenWrittenAsSumOfTwoAbundantNumbers = false; //Will be set to true only if at least one sum can be expressed using abundant numbers.
                long midpoint = (number / 2) + 1;
                for (int i = 1; i <= midpoint; i++)
                {
                    #region Get left and right portions of the number in all combinations
                    var left = i;
                    var right = number - left;
                    #endregion
                    var leftAbundancyType = numberDivisor.IsAbundantOrDeficientOrPerfect(left);
                    var rightAbundancyType = numberDivisor.IsAbundantOrDeficientOrPerfect(right);
                    if (leftAbundancyType == AbundancyType.Abundant && rightAbundancyType == AbundancyType.Abundant)
                    {
                        hasBeenWrittenAsSumOfTwoAbundantNumbers = true;
                        break;
                    }
                }
                if (!hasBeenWrittenAsSumOfTwoAbundantNumbers)
                {
                    lock (this) summator += number; //Increase the sum if combination of 2 abundant numbers could not be found.
                }

            });


            answer = string.Format("Sum of all the positive integers which cannot be written as the sum of two abundant numbers is {0}.", summator);
        }

        public void TestPerfects()
        {
            DateTime start = DateTime.Now;

            long temp = 28;
            NumberDivisor numberDivisor = new NumberDivisor();
            var sumDivisors = numberDivisor.CalculateSumOfProperDivisors(temp);

            var is28Abundant = numberDivisor.IsAbundantOrDeficientOrPerfect(temp);
            var is12Abundant = numberDivisor.IsAbundantOrDeficientOrPerfect(12);
            var is57Abundant = numberDivisor.IsAbundantOrDeficientOrPerfect(57);

            var elapsedTime = DateTime.Now - start;
            var sb = new StringBuilder();
            sb.AppendFormat("Elapsed computation time: {0}. Sum of properDivisors of {1} = {2}.", elapsedTime, temp, sumDivisors); sb.AppendLine();
            sb.AppendFormat("Number {0} is {1}.", temp, is28Abundant); sb.AppendLine();
            sb.AppendFormat("Number {0} is {1}.", 12, is12Abundant); sb.AppendLine();
            sb.AppendFormat("Number {0} is {1}.", 57, is57Abundant); sb.AppendLine();
            Answer = sb.ToString();
        }
    }
}
