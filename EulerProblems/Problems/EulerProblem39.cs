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
    [ProblemSolver("Integer right triangles", displayName = "Problem 39", problemDefinition =
@"If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, there are exactly three solutions for p = 120.

{20,48,52}, {24,45,51}, {30,40,50}

For which value of p ≤ 1000, is the number of solutions maximised?"
)]
    public class EulerProblem39 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var triangles = new List<RightTriangle>();

            var test = new RightTriangle(3, 4);
            var isInteger = test.IsInteger;
            test = new RightTriangle(4, 7);
            isInteger = test.IsInteger;

            //Check all triangles with a ≤ 1000
            Parallelization.GetParallelRanges(1, 1_000, 4).ForAll(sequence =>
             {
                 foreach (int a in sequence)
                 {
                     //Check all triangles with b ≤ 1000
                     foreach (int b in Enumerable.Range(1, 1_000))
                     {
                         var triangle = new RightTriangle(a, b);
                         if (triangle.IsInteger)
                         {
                             if (triangle.Perimeter <= 1000)
                             {
                                 lock (this) triangles.Add(triangle);
                             }
                         }
                     }
                 }

             });
            //All right triangles are in the triangles list.
            //Group them by perimeter
            var groups = triangles.GroupBy(triangle => triangle.Perimeter);
            //Sort to find the highest count of solutions.
            var orderedGroups = groups.OrderByDescending(group => group.Count());
            var solution = orderedGroups.First();
            answer = $"Computing... Count={triangles.Count}. Solution = P={solution.Key} (possible solutions: {solution.Count()}).";
        }
        /// <summary>
        /// Model representing a number and its pandigital representation
        /// </summary>
        internal class RightTriangle
        {
            private int A;
            private int B;
            public int C { get; }
            public int Perimeter => A + B + C;

            public RightTriangle(int a, int b)
            {
                A = a;
                B = b;
                var a2b2 = A * A + B * B;
                C = (int)Math.Sqrt(a2b2);
            }
            /// <summary>
            /// Checks if this is right triable whose sides {a, b, c} have integer lengths.
            /// </summary>
            public bool IsInteger => A * A + B * B == C * C;
            public override string ToString()
            {
                return $"A={A}, B={B}, C={C}, P={Perimeter}";
            }
        }
    }
}
