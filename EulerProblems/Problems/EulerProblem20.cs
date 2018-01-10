using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// n! means n × (n − 1) × ... × 3 × 2 × 1

    /// For example, 10! = 10 × 9 × ... × 3 × 2 × 1 = 3628800,
    /// and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27.
    /// 
    /// Find the sum of the digits in the number 100!
    /// </summary>
    public class EulerProblem20 : AbstractEulerProblem
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;

            BigInteger ten = 10;

            BigInteger number = 1;
            foreach (int power in Enumerable.Range(1, 100))
            {
                number *= power;
            }

            int sumOfDigits = 0;
            BigInteger searcherValue = number;
            while (searcherValue != 0)
            {
                int reminder = (searcherValue % ten).IntValue();
                sumOfDigits += reminder;
                searcherValue /= ten;
            }

            var elapsedTime = DateTime.Now - start;
            Answer = string.Format("Elapsed computation time: {0}. 100! is equal to: {1}. The sum of the digits in the number 100! is equal to {2}.", elapsedTime, number, sumOfDigits);
        }
    }
}
