using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems.Problems30to39
{
    /// <summary/>
    [ProblemSolver("Truncatable primes", displayName = "Problem 37", problemDefinition =
@"The number 3797 has an interesting property. Being prime itself, it is possible to continuously remove digits from left to right, and remain prime at each stage: 3797, 797, 97, and 7. Similarly we can work from right to left: 3797, 379, 37, and 3.

Find the sum of the only eleven primes that are both truncatable from left to right and right to left.

NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes."
        )]
    public class EulerProblem37 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int sum = 0;
            PrimeSolver solver = new PrimeSolver();
            List<Representation> list = new List<Representation>();
            Parallelization.GetParallelRanges(1, 10_000_000, 100).ForAll(sequence =>
            {
                foreach (int number in sequence)
                {
                    var rep = new Representation(solver, number);
                    if (rep.IsPrime())
                    {
                        if (rep.IsTruncatable())
                        {
                            lock (this) list.Add(rep);
                        }
                    }
                }
            }
            );
            sum = list.Sum(el => el.Number);
            StringBuilder DEBUGString = new StringBuilder().AppendLine();
            list.ForEach(item => DEBUGString.Append($"{item}, "));
            answer = $"Computing... Sum = {sum}. List size = {list.Count}. {DEBUGString}";
        }

        /// <summary>
        /// Representation of a number (potential prime) containing methods for easy manipulation and checks.
        /// </summary>
        private class Representation
        {
            private PrimeSolver solver;
            public int Number { get; private set; }

            public Representation(PrimeSolver solverInstance, int number)
            {
                solver = solverInstance;
                Number = number;
            }

            /// <summary>
            /// Checks for primeness of this number.
            /// </summary>
            internal bool IsPrime() => solver.IsPrime(Number);
            private bool IsPrime(long num) => solver.IsPrime(num);

            /// <summary>
            /// Makes iterations for each string representation to ensure it is also a prime.
            /// Returns true if all truncations are also primes.
            /// </summary>
            internal bool IsTruncatable()
            {
                if (!IsPrime()) return false; //Check necessary condition.

                var digitArray = Number.ToString().ToArray(); //Convert to array of digits
                var digitLength = digitArray.Count();
                if (digitLength == 1) return false; //Single-digit primes are considered to be non-truncatable by definition.

                //First truncate from the left
                for (int index = 1; index < digitLength; index++)
                {
                    long testInt = MoreMath.IntFromDigits(digitArray.Skip(index));
                    if (!IsPrime(testInt)) return false;
                }

                //Now trunctate from the right
                for (int index = 1; index < digitLength; index++)
                {
                    long testInt = MoreMath.IntFromDigits(digitArray.Take(index));
                    if (!IsPrime(testInt)) return false;
                }

                return true;
            }
            public override string ToString()
            {
                return $"{Number}";
            }
        }
    }
}
