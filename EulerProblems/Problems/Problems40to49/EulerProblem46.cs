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
    [ProblemSolver("Goldbach's other conjecture", "Problem 46",
@"It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.
 9 = 7 + 2×1^2
 15 = 7 + 2×2^2
 21 = 3 + 2×3^2
 25 = 7 + 2×3^2
 27 = 19 + 2×2^2
 33 = 31 + 2×1^2
It turns out that the conjecture was false.
What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?")]
    public class EulerProblem46 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            answer = $"Solution not created yet...";
            StringBuilder progress = new StringBuilder("Composites checked: ");
            CompositeSolver compositeSolver = new CompositeSolver();
            List<long> oddComposites = new List<long>();
            //Pseudo code
            foreach (long candidate in Enumerable64.Range(1, 34000))
            {
                if (compositeSolver.IsOddComposite(candidate))
                {
                    oddComposites.Add(candidate);
                    if (compositeSolver.IsSumOfPrimeAndTwiceSquare(candidate))
                    {
                        progress.Append($"{candidate}, ");
                        //UpdateProgress(progress.ToString());
                    }
                    else
                    {
                        answer = $"The smallest odd composite that cannot be written as the sum of a prime and twice a square is: {candidate}.";
                        return;
                    }

                }
            }
        }

        private class CompositeSolver
        {
            /// <summary>
            /// Internal helper class for primes.
            /// </summary>
            private readonly PrimeSolver primeSolver = new PrimeSolver();
            public CompositeSolver()
            {
            }

            /// <summary>
            /// Returns true if the candiate is an odd composite number. Otherwise it returns false.
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns>True if the candiate is an odd composite number. False otherwise.</returns>
            internal bool IsOddComposite(long candidate)
            {
                if (MoreMath.IsEven(candidate)) return false;
                return IsComposite(candidate);
            }

            /// <summary>
            /// Checks if candidate can be expressed as [n = p + 2 * a^2], where n is <paramref name="candidate"/>, p is some prime and 'a' is an integer.
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns>Returns true if it can be expressed as p + 2 * a^2. False otherwise.</returns>
            internal bool IsSumOfPrimeAndTwiceSquare(long candidate)
            {
                var primes = primeSolver.GetPrimesSmallerThan(candidate);
                foreach (var prime in primes)
                {
                    foreach (long a in Enumerable64.Range(1, candidate / 2))
                    {
                        var twiceASquared = 2 * a * a;
                        if (candidate == prime + twiceASquared)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            /// <summary>
            /// Returns true if the candidate is a composite number. Otherwise it returns false.
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns>True if the candiate is a composite number. False otherwise.</returns>
            private bool IsComposite(long candidate)
            {
                var divisors = primeSolver.GetDivisors(candidate);
                if (divisors.Count < 3) return false; //If the candidate has only two divisors (1 and itself), then it is ont composite.
                return true;
            }
        }
    }

}
