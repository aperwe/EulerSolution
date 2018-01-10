using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// 2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.
    /// What is the sum of the digits of the number 2^1000?
    /// </summary>
    public class EulerProblem16 : AbstractEulerProblem
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;

            BigInteger power = 1;
            foreach(int iterator in Enumerable.Range(1, 1000))
            {
                power *= 2;

            }

            string stringOfDigits = power.ToString();
            int sum = 0;
            foreach(char c in stringOfDigits)
            {
                var digit = int.Parse(c.ToString());
                sum += digit;
            }
            var elapsedTime = DateTime.Now - start;
            Answer = string.Format("Elapsed computation time: {0}. 2^1000 = {1}. Sum of digits = {2}.", elapsedTime, power, sum);
        }
    }
}
