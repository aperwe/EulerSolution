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
            int fact2 = Factorial(2);
            int fact5 = Factorial(5);
            int sum = 0;
            //TODO: Is this upper limit enough?
            foreach (int number in Enumerable.Range(3, 100000))
            {
                //TODO, break each number ito digits
                //TODO: get factorials of each digit
                //TODO: sum factorials
                //TODO: compare sum(factorials) with the input number.
                //TODO: If they are equal, add number to the sum.
            }
            answer = $"Computing... {fact2}, {fact5}. Answer is {sum}.";
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
