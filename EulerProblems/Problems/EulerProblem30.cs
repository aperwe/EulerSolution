using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Fibonacci;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits:
    /// 1634 = 1^4 + 6^4 + 3^4 + 4^4
    /// 8208 = 8^4 + 2^4 + 0^4 + 8^4
    /// 9474 = 9^4 + 4^4 + 7^4 + 4^4
    /// As 1 = 1^4 is not a sum it is not included.
    /// 
    /// The sum of these numbers is 1634 + 8208 + 9474 = 19316.
    /// Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.
    /// </summary>
    [ProblemSolverClass("Problem 30", DisplayName = "Problem 30")]
    public class EulerProblem30 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            List<Number> longList = new List<Number>();
            var range = Enumerable.Range(0, 10);
            range.AsParallel().ForAll(hundredThousands =>
            {
                foreach (int tenThousands in range)
                {
                    foreach (int thousands in range)
                    {
                        foreach (int hundreds in range)
                        {
                            foreach (int tens in range)
                            {
                                foreach (int ones in range)
                                {
                                    var pair = new Number(hundredThousands, tenThousands, thousands, hundreds, tens, ones);
                                    pair.CalcPow(5);
                                    if (pair.Value == pair.SumOfPowers)
                                    {
                                        lock (longList)
                                        {
                                            longList.Add(pair);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var allEquals = from number in longList
                            where number.Value == number.SumOfPowers && number.Value != 1
                            select number.Value;
            var allEqualsList = allEquals.ToArray();
            var sum = allEquals.Sum();
            answer = string.Format("Sum of all the numbers that can be written as the sum of fifth powers of their digits is: {0}.", sum);
        }

        class Number
        {
            int hundredThousands;
            int tenThousands;
            int thousands;
            int hundreds;
            int tens;
            int ones;

            public int Value { get; }

            private BigInteger sumOfPowers;
            internal BigInteger SumOfPowers => sumOfPowers;

            public Number(int hundredThousands, int tenThousands, int thousands, int hundreds, int tens, int ones)
            {
                this.hundredThousands = hundredThousands;
                this.tenThousands = tenThousands;
                this.thousands = thousands;
                this.hundreds = hundreds;
                this.tens = tens;
                this.ones = ones;
                Value = CalcValue;
            }

            internal void CalcPow(int power)
            {
                BigInteger sum = new BigInteger(hundredThousands).Power(power);
                sum += new BigInteger(tenThousands).Power(power);
                sum += new BigInteger(thousands).Power(power);
                sum += new BigInteger(hundreds).Power(power);
                sum += new BigInteger(tens).Power(power);
                sum += new BigInteger(ones).Power(power);
                sumOfPowers = sum;
            }
            private int CalcValue =>
                hundredThousands * 100000 +
                tenThousands * 10000 +
                thousands * 1000 +
                hundreds * 100 +
                tens * 10 +
                ones;
        }
    }

}
