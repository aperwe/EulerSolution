using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary/>
    [ProblemSolver("Champernowne's constant", displayName = "Problem 40", problemDefinition =
@"An irrational decimal fraction is created by concatenating the positive integers:

     0.123456789101112131415161718192021...

It can be seen that the 12th digit of the fractional part is 1.

If dn represents the nth digit of the fractional part, find the value of the following expression.

      d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000"
)]
    public class EulerProblem40 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            List<int> dnDigits = new List<int>();
            //Create fractional part
            StringBuilder fraction = new StringBuilder();
            foreach (int integer in Enumerable.Range(1, 1_000_003))
            {
                fraction.Append(integer);
            }
            int initialPosition = 1;
            int multiplier = 10;
            for (int selector = initialPosition; selector <= 1_000_000; selector *= multiplier)
            {
                var nthDigit = fraction[selector - 1];
                dnDigits.Add(nthDigit - '0');
            }
            //Now all digits to multiply are in dnDigits list. Multiply them to get the result.
            var result = dnDigits.Aggregate(1, (a, b) => a * b);

            answer = $"Computing... Fraction length {fraction.Length}, Result = {result}";
        }
    }
}
