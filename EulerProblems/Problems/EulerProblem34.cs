using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// 145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145.
    /// Find the sum of all numbers which are equal to the sum of the factorial of their digits.
    /// Note: as 1! = 1 and 2! = 2 are not sums they are not included.
    /// </summary>
    [ProblemSolver("Problem 34", displayName = "Problem 34")]
    public class EulerProblem34 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int sum = 0;
            List<int> items = new List<int>();
            foreach (int number in Enumerable.Range(3, 10_000_000))
            {
                //break each number ito digits
                var digitArray = from digit in number.ToString().ToArray()
                                 select digit - '0';
                //get factorials of each digit
                var factorialArray = from digit in digitArray
                                     select MoreMath.Factorial(digit);
                //sum factorials
                var sumFactorials = factorialArray.Sum();
                //compare sum(factorials) with the input number.
                if (number == sumFactorials)
                {
                    sum += sumFactorials;
                    items.Add(number);
                }
            }
            StringBuilder DEBUGString = new StringBuilder().AppendLine();
            items.ForEach(item => DEBUGString.Append($"{item}; "));
            answer = $"Computing... Answer is {sum}. Count items is {items.Count}. Items: {DEBUGString.ToString()}";
        }
    }
}
