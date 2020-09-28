using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems
{
    /// <summary>
    /// It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.
    /// 9 = 7 + 2×12
    /// 15 = 7 + 2×22
    /// 21 = 3 + 2×32
    /// 25 = 7 + 2×32
    /// 27 = 19 + 2×22
    /// 33 = 31 + 2×12
    /// It turns out that the conjecture was false.
    /// What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?
    /// </summary>
    [ProblemSolverClass("Goldbach's other conjecture", DisplayName = "Problem 45 parallel")]
    public class EulerProblem46 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var triangleNumbers = new Dictionary<long, long>();
            var pentagonalNumbers = new Dictionary<long, long>();
            var hexagonalNumbers = new Dictionary<long, long>();
            var maxN = 100000;
            GenerateNumbers(1, maxN, triangleNumbers, pentagonalNumbers, hexagonalNumbers);
            var results = new List<long>();

            Parallel.ForEach(triangleNumbers, (pair) =>
            {
                var currentTriangle = pair.Value;
                if (IsPentagonal(currentTriangle, pentagonalNumbers))
                {
                    if (IsHexagonal(currentTriangle, hexagonalNumbers))
                    {
                        lock (results) //async
                        {
                            results.Add(pair.Key);
                            results.Add(currentTriangle);
                        }
                    }
                }
            });

            var strings = results.Select(n => n.ToString());
            var joinedStrings = string.Join("; ", strings);
            answer = $"In range of n = {maxN}: {joinedStrings}...";
        }

        private bool IsPentagonal(long triangleValue, Dictionary<long, long> pentagonalNumbers) => pentagonalNumbers.ContainsValue(triangleValue);
        private bool IsHexagonal(long triangleValue, Dictionary<long, long> hexagonalNumbers) => hexagonalNumbers.ContainsValue(triangleValue);

        private void GenerateNumbers(long min, long max, Dictionary<long, long> triangleNumbers, Dictionary<long, long> pentagonalNumbers, Dictionary<long, long> hexagonalNumbers)
        {
            foreach (long n in Enumerable64.Range(min, max - min))
            {
                triangleNumbers.Add(n, n * (n + 1) / 2);
                pentagonalNumbers.Add(n, n * (3 * n - 1) / 2);
                hexagonalNumbers.Add(n, n * (2 * n - 1));
            }
        }
    }

}
