using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems00to09
{
    /// <summary/>
    [ProblemSolver("Problem 6", displayName = "Problem 06",
        problemDefinition = 
        @"The sum of the squares of the first ten natural numbers is,
 1^2 + 2^2 + ... + 10^2 = 385
The square of the sum of the first ten natural numbers is,
 (1 + 2 + ... + 10)^2 = 55^2 = 3025
Hence the difference between the sum of the squares of the first ten natural numbers and the square of the sum is 3025 − 385 = 2640.
Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.")]
    public class EulerProblem6 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            Func<SumCalculator, string> format = (sc) => string.Format("n={0}, Sum of squares = {1}, Square of sum = {2}, Diff = {3}", sc.n, sc.SumOfSquares, sc.SquareOfSum, sc.Diff);

            SumCalculator test = new SumCalculator(1);
            foreach (int iterator in Enumerable.Range(2,99))
            {
                test = new SumCalculator(test);
                //var p = format(test);
            }
            var stringAnswer = format(test);
            string another = stringAnswer;

            SumCalculator method2Answer = new SumCalculator(100);
            string method2string = format(method2Answer);

            var compare = string.Compare(another, method2string);
            //both strings should be equal, if both methods work the same way.
            if (compare != 0) throw new InvalidOperationException("Unit test failed. 2 methods for calculating the answer do not produce the same result.");
            answer = string.Format("The difference between the sum of the squares of the first one hundred natural numbers and the square of the sum is: {0}.", method2string);
        }

        internal class SumCalculator
        {
            public SumCalculator(long n)
            {
                if (n < 1) throw new ArgumentOutOfRangeException("n");
                this.n = n;
            }
            public SumCalculator(SumCalculator previous)
            {
                this.n = previous.n + 1;
                this.SumOfSquares = previous.SumOfSquares + (this.n * this.n);
                //Take advantage of the fact that (a + b)^2 = a^2 + 2ab + b^2
                this.Sum = previous.Sum + this.n;
                this.SquareOfSum = previous.SquareOfSum + (2 * previous.Sum * this.n) + (this.n * this.n);
            }

            public long n { get; private set; }

            #region Logic related to sum of squares
            long _SumOfSquares;
            public long SumOfSquares
            {
                get
                {
                    if (_SumOfSquares == 0) _SumOfSquares = CalculateSumOfSquares(); return _SumOfSquares;
                }
                private set { _SumOfSquares = value; }
            }
            long CalculateSumOfSquares()
            {
                long output = 0;
                foreach (int iterator in Enumerable.Range(1, (int)n))
                {
                    output += iterator * iterator;
                }
                return output;
            }

            #endregion

            #region Logic related to square of sum
            long _SquareOfSum;
            public long SquareOfSum
            {
                get
                {
                    if (_SquareOfSum == 0) _SquareOfSum = CalculateSquareOfSum(); return _SquareOfSum;
                }
                private set { _SquareOfSum = value; }
            }

            private long _Sum;
            private long Sum
            {
                get
                {
                    if (_Sum == 0) _Sum = Enumerable.Range(1, (int)n).Sum();
                    return _Sum;
                }
                set
                {
                    _Sum = value;
                }
            }
            private long CalculateSquareOfSum()
            {
                long output = 0;
                output = Sum * Sum;
                return output;
            }

            #endregion

            public long Diff { get { return SquareOfSum - SumOfSquares; } }

        }
    }
}
