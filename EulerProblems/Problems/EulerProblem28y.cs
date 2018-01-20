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
    /// <summary>Starting with the number 1 and moving to the right in a clockwise direction a 5 by 5 spiral is formed as follows:
    /// 21 22 23 24 25
    /// 20  7  8  9 10
    /// 19  6  1  2 11
    /// 18  5  4  3 12
    /// 17 16 15 14 13
    /// It can be verified that the sum of the numbers on the diagonals is 101.
    /// What is the sum of the numbers on the diagonals in a 1001 by 1001 spiral formed in the same way?</summary>
    [ProblemSolverClass("Problem 28", DisplayName = "Problem 28")]
    public class EulerProblem28 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var sum = Method1Solution();
            answer = string.Format("Still computing. Answer = {0}.", sum);
            //sum = Method1BruteForceSolution();
            //answer = string.Format("Still computing. Answer = {0}.", sum);
        }

        /// <summary>Clever alogorithmic method. Not using arrays.</summary>
        private string Method1Solution()
        {
            SpiralDiagonals sd = new SpiralDiagonals(seed: 1);
            while (sd.ArraySize < 5) sd.DoOneStep();
            while (sd.ArraySize < 1001) sd.DoOneStep();

            return sd.ToString();
        }
        private string Method1BruteForceSolution()
        {
            return null;
        }

    }
    class SpiralDiagonals
    {
        private long seed;
        private long arraySize;
        private long answer;
        private long counter;
        private int step;

        public long Answer => answer;
        public long ArraySize => arraySize - 1;

        public SpiralDiagonals(long seed)
        {
            this.seed = seed;
            arraySize = 1 - 1; //Size is 1, but remove 1 for the corner (needed when adding)
            answer = 0;
            counter = seed;
            step = 0;
        }

        internal void DoStep(int numSteps)
        {
            foreach (int step in Enumerable.Range(1, numSteps)) DoOneStep();
        }
        internal void DoOneStep()
        {
            if (step == 0)
            {
                answer = counter;
                arraySize += 2; //With each step, the array grows by 2 (1 on each side (left-right and top-bottom)
            }
            else
            {
                counter += arraySize; answer += counter; //bottom-right
                counter += arraySize; answer += counter; //bottom-left
                counter += arraySize; answer += counter; //top-left
                counter += arraySize; answer += counter; //top-right
                arraySize += 2;
            }
            step++;
        }
        public override string ToString() => string.Format("Answer: {0}, Array size: {1}", answer, ArraySize);
    }
}
