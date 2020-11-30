using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.Problems40to49
{
    /// <summary/>
    [ProblemSolver("Self powers", "Problem 48",
@"The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317.

Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000.")]
    public class EulerProblem48 : AbstractEulerProblem
    {
        PrimeSolver primeSolver = new PrimeSolver();
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
            BigInteger bigInteger = 0;
            foreach(BigInteger current in Enumerable.Range(1, 1000))
            {
                bigInteger += current.Power(current);
            }
            var tenLastDigits = new string(bigInteger.ToString().Reverse().Take(10).Reverse().ToArray());
            answer = $"Solution: '{tenLastDigits}'";
        }

    }
}