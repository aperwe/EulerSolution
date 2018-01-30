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
    [ProblemSolverClass("Problem 34", DisplayName = "Problem 34")]
    public class EulerProblem34 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int sum = 0;
            List<int> items = new List<int>();
            //TODO: Is this upper limit enough? Factorial of 9! = 362880.
            foreach (int number in Enumerable.Range(3, 10_000_000))
            {
                //TODO, break each number ito digits
                var digitArray = from digit in number.ToString().ToArray()
                                 select digit - '0';
                //TODO: get factorials of each digit
                var factorialArray = from digit in digitArray
                                     select Factorial(digit);
                //TODO: sum factorials
                var sumFactorials = factorialArray.Sum();
                //TODO: compare sum(factorials) with the input number.
                if (number == sumFactorials)
                {
                    //TODO: If they are equal, add number to the sum.
                    sum += sumFactorials;
                    items.Add(number);
                }
            }
            StringBuilder DEBUGString = new StringBuilder().AppendLine();
            items.ForEach(item => DEBUGString.Append($"{item}; "));
            answer = $"Computing... Answer is {sum}. Count items is {items.Count}. Items: {DEBUGString.ToString()}";
        }
        /// <summary>
        /// Produces a factorial for the specified number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Factorial</returns>
        public int Factorial(int number)
        {
            //Axioms
            if (number < 0) return 0;
            if (number == 0) return 1;
            if (number == 1) return 1;
            //Otherwise iteratively calculate the factorial
            int retVal = 1;
            while (number > 1) retVal *= number--;
            return retVal;
        }
    }
}
