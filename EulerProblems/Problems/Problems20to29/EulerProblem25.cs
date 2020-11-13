using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Fibonacci;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary/>
    [ProblemSolver("1000-digit Fibonacci number", displayName = "Problem 25", problemDefinition =
@"The Fibonacci sequence is defined by the recurrence relation:

   Fn = Fn−1 + Fn−2, where F1 = 1 and F2 = 1.
Hence the first 12 terms will be:

   F1 = 1
   F2 = 1
   F3 = 2
   F4 = 3
   F5 = 5
   F6 = 8
   F7 = 13
   F8 = 21
   F9 = 34
   F10 = 55
   F11 = 89
   F12 = 144
The 12th term, F12, is the first term to contain three digits.

What is the index of the first term in the Fibonacci sequence to contain 1000 digits?"
        )]
    public class EulerProblem25 : AbstractEulerProblem
    {

        protected override void Solve(out string answer)
        {
            int index = 0;
            var fibonacciSequence = new FibonacciSequenceBig();

            while (true)
            {
                index++;

                var fx = fibonacciSequence.Get(index - 1);
                var str = fx.ToString();
                var len = str.Length;
                if (len > 999)
                {
                    answer = string.Format("The index of the first term in the Fibonacci sequence to contain 1000 digits is: {0}. Number is: {1}.", index, str);

                    break;
                }
            }
        }
    }
}
