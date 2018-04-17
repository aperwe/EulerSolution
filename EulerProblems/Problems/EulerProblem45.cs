﻿using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// Triangle, pentagonal, and hexagonal numbers are generated by the following formulae:
    /// Triangle   Tn = n(n + 1) / 2     1, 3, 6, 10, 15, ...
    /// Pentagonal Pn = n(3n−1) / 2      1, 5, 12, 22, 35, ...
    /// Hexagonal  Hn = n(2n−1)          1, 6, 15, 28, 45, ...
    /// 
    /// It can be verified that T285 = P165 = H143 = 40755.
    /// Find the next triangle number that is also pentagonal and hexagonal.
    /// </summary>
    [ProblemSolverClass("Triangular, pentagonal and hexagonal numbers", DisplayName = "Problem 45")]
    public class EulerProblem45 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var triangleNumbers = new Dictionary<long, long>();
            var pentagonalNumbers = new Dictionary<long, long>();
            var hexagonalNumbers = new Dictionary<long, long>();
            var maxN = 100000;
            GenerateNumbers(1, maxN, triangleNumbers, pentagonalNumbers, hexagonalNumbers);
            var results = new List<long>();
            foreach (var pair in triangleNumbers)
            {
                var currentTriangle = pair.Value;
                if (IsPentagonal(currentTriangle, pentagonalNumbers))
                {
                    if (IsHexagonal(currentTriangle, hexagonalNumbers))
                    {
                        results.Add(pair.Key);
                        results.Add(currentTriangle);
                    }
                }
            }

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