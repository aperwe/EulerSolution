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
    [ProblemSolverClass("Problem 28", DisplayName = "Problem 28a")]
    public class EulerProblem28a : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var sum = Method1Solution();
            answer = string.Format("Answer = {0}.", sum);
        }

        /// <summary>Clever alogorithmic method. Not using arrays.</summary>
        private string Method1Solution()
        {
            SpiralDiagonals sd = new SpiralDiagonals(seed: 1);
            while (sd.ArraySize < 5) sd.DoOneStep();
            while (sd.ArraySize < 1001) sd.DoOneStep();

            return sd.ToString();
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

    [ProblemSolverClass("Problem 28", DisplayName = "Problem 28b")]
    public class EulerProblem28b : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            var sum = Method2BruteForceSolution();
            answer = string.Format("Answer = {0}.", sum);
        }

        private string Method2BruteForceSolution()
        {
            int maxX = 1001;
            int maxY = 1001;
            int[,] data = new int[maxX, maxY];
            int midX = maxX / 2, midY = maxY / 2;
            int seed = 1;

            int currentX = midX, currentY = midY;

            Go(currentX, currentY, seed, data);
            string sum = FindSumBruteForce(data);

            return string.Format("Answer: {0}, Array size: {1}", sum, maxX);
        }

        private string FindSumBruteForce(int[,] data)
        {
            int xExtent = data.GetUpperBound(0);
            int yExtent = data.GetUpperBound(1);

            int sum = 0;

            //Sum over top-left to bottom-right diagonal
            foreach (int iterator in Enumerable.Range(0, xExtent + 1))
            {
                sum += data[iterator, iterator];
            }
            //Sum over bottom-left to top-right diagonal
            foreach (int iterator in Enumerable.Range(0, xExtent + 1))
            {
                sum += data[xExtent - iterator, iterator];
            }
            //Remove the middle part (counted twice on both diagonals)
            sum -= data[xExtent / 2, yExtent / 2];
            return sum.ToString();
        }

        private void Go(int startX, int startY, int seed, int[,] data)
        {
            int currentX = startX, currentY = startY;
            Direction direction = Direction.Right; //Starting direction.
            int currentValue = seed;

            #region Lambdas
            Action moveSomewhere = () =>
            {
                switch (direction)
                {
                    case Direction.Right: currentX++; break;
                    case Direction.Down: currentY++; break;
                    case Direction.Left: currentX--; break;
                    case Direction.Up: currentY--; break;
                }
            };
            Action undoMoveSomewhere = () =>
            {
                switch (direction)
                {
                    case Direction.Right: currentX--; break;
                    case Direction.Down: currentY--; break;
                    case Direction.Left: currentX++; break;
                    case Direction.Up: currentY++; break;
                }
            };
            Action changeDirection = () =>
            {
                //Change direction
                switch (direction)
                {
                    case Direction.Right: direction = Direction.Down; break;
                    case Direction.Down: direction = Direction.Left; break;
                    case Direction.Left: direction = Direction.Up; break;
                    case Direction.Up: direction = Direction.Right; break;
                }
            };
            #endregion

            int maxSteps = 1;
            const int maxDirections = 4;
            Displayer dp = new Displayer(data);

            #region Initial step #1 to be done outside the loop
            data[currentX, currentY] = currentValue++;
            moveSomewhere();
            changeDirection();
            maxSteps++; //Should be 2
            #endregion

            foreach (int iteration in Enumerable.Range(0, startX)) //Number of spiral revolutions (one is done above, outside the loop)
            {
                foreach (int dir in Enumerable.Range(1, maxDirections))
                {
                    foreach (int step in Enumerable.Range(1, maxSteps))
                    {
                        data[currentX, currentY] = currentValue++;
                        moveSomewhere();
                    }
                    undoMoveSomewhere(); //When changing direction, important thing is to undo the last move.
                    changeDirection(); // If you change direction, undo move somewhere
                    moveSomewhere(); //And then move in the new direction
                }
                undoMoveSomewhere();
                changeDirection();
                changeDirection();
                changeDirection();  //Change 3 times is the same as changing 1 time counter-clockwise
                moveSomewhere();
                changeDirection(); //Reset to the direction from the previous inner loop.
                maxSteps += 2; //Next spiral revolution is 2 element larger
            }
        }
        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
    }
    class Displayer
    {
        private int[,] data;

        internal Displayer(int[,] data)
        {
            this.data = data;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int y in Enumerable.Range(0, data.GetUpperBound(0) + 1))
            {
                foreach (int x in Enumerable.Range(0, data.GetUpperBound(1) + 1))
                {
                    sb.AppendFormat("{0}, ", data[x, y]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
