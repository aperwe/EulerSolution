using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// Starting in the top left corner of a 2×2 grid, and only being able to move to the right and down, there are exactly 6 routes to the bottom right corner.
    /// How many such routes are there through a 20×20 grid?
    /// </summary>
    public class EulerProblem15 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            int gridSize = 20;
            //BigInteger answer = 1;
            //foreach (int iterator in Enumerable.Range(2, (int)gridSize))
            //{
            //    answer *= iterator;
            //}
            //foreach (int iterator in Enumerable.Range(1, (int)gridSize - 1))
            //{
            //    answer *= iterator;
            //}

            LagrangeManipulator manipulator = new LagrangeManipulator();
            foreach (int iterator in Enumerable.Range(1, (gridSize * 2) + 1))
            {
                manipulator.CalculateNewRow();

            }

            answer = string.Format("Through {0}x{0} grid there are exactly {1} routes.", gridSize, manipulator.CalculatePaths());
        }

        private void Recalculate(List<Lagrange> lagranges)
        {
            foreach (int index in Enumerable.Range(0, lagranges.Count - 1))
            {
                var current = lagranges[index];
                var next = lagranges[index + 1];
                next.combinations += current.combinations;
            }
        }

        private class Lagrange
        {
            internal int row;
            internal long combinations;
            public override string ToString()
            {
                return string.Format("Combinations {0}", combinations);
            }
        }
        private class LagrangeManipulator
        {
            List<Lagrange> previousRow;
            List<Lagrange> newRow;
            List<List<Lagrange>> allRows = new List<List<Lagrange>>();
            internal void CalculateNewRow()
            {
                previousRow = newRow;
                int newRowWidth = GetPreviousRowSize() + 1;
                newRow = new List<Lagrange>();
                foreach (int iterator in Enumerable.Range(0, newRowWidth))
                {
                    long newValue = GetLeftOf(iterator) + GetRightOf(iterator);
                    //special case for initial (top) row
                    if (newValue == 0) newValue = 1;
                    Lagrange newLagrange = new Lagrange { combinations = newValue };
                    newRow.Add(newLagrange);
                }
                allRows.Add(newRow);
            }

            private int GetPreviousRowSize()
            {
                if (previousRow == null) return 0;
                return previousRow.Count;
            }

            private long GetLeftOf(int iterator)
            {
                if (iterator == 0) return 0;
                return previousRow[iterator - 1].combinations;
            }

            private long GetRightOf(int iterator)
            {
                if (iterator >= GetPreviousRowSize()) return 0;
                return previousRow[iterator].combinations;
            }

            internal long CalculatePaths()
            {
                //allRows contains the widest row as its last entry
                if (newRow == null) return 0;
                int size = newRow.Count;
                int midpoint = (size) / 2; //for odd counts (expected), this will give exactly the mid point location.
                return newRow[midpoint].combinations;
            }
        }
    }
}
