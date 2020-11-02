using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// By starting at the top of the triangle below and moving to adjacent numbers on the row below, the maximum total from top to bottom is 23.

    /// 3
    /// 7 4
    /// 2 4 6
    /// 8 5 9 3

    /// That is, 3 + 7 + 4 + 9 = 23.

    /// Find the maximum total from top to bottom of the triangle below:

    /// 75
    /// 95 64
    /// 17 47 82
    /// 18 35 87 10
    /// 20 04 82 47 65
    /// 19 01 23 75 03 34
    /// 88 02 77 73 07 63 67
    /// 99 65 04 28 06 16 70 92
    /// 41 41 26 56 83 40 80 70 33
    /// 41 48 72 33 47 32 37 16 94 29
    /// 53 71 44 65 25 43 91 52 97 51 14
    /// 70 11 33 28 77 73 17 78 39 68 17 57
    /// 91 71 52 38 17 14 91 43 58 50 27 29 48
    /// 63 66 04 68 89 53 67 30 73 16 69 87 40 31
    /// 04 62 98 27 23 09 70 98 73 93 38 53 60 04 23

    /// NOTE: As there are only 16384 routes, it is possible to solve this problem by trying every route.However, Problem 67, is the same challenge with a triangle containing one-hundred rows; it cannot be solved by brute force, and requires a clever method! ;o)
    /// </summary>
    [ProblemSolver("Problem 18", displayName = "Problem 18")]
    public class EulerProblem18 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            string[] testData = {
                            "3",
                            "7 4",
                            "2 4 6",
                            "8 5 9 3" };

            var max1 = FindMaximum(testData);

            string[] actualData =
            {
                            "75",
                            "95 64",
                            "17 47 82",
                            "18 35 87 10",
                            "20 04 82 47 65",
                            "19 01 23 75 03 34",
                            "88 02 77 73 07 63 67",
                            "99 65 04 28 06 16 70 92",
                            "41 41 26 56 83 40 80 70 33",
                            "41 48 72 33 47 32 37 16 94 29",
                            "53 71 44 65 25 43 91 52 97 51 14",
                            "70 11 33 28 77 73 17 78 39 68 17 57",
                            "91 71 52 38 17 14 91 43 58 50 27 29 48",
                            "63 66 04 68 89 53 67 30 73 16 69 87 40 31",
                            "04 62 98 27 23 09 70 98 73 93 38 53 60 04 23"
            };

            var max2 = FindMaximum(actualData);
            answer = string.Format("Maximum total from top to bottom of the triangle is {0}.", max2);
        }

        internal int FindMaximum(string[] inputData)
        {
            Tree tree = ConvertStringDataToTree(inputData);
            tree.FindChildren();
            var treeCount = tree.ToString();
            int max = tree.FindMaximumTotal();
            return max;
        }

        private Tree ConvertStringDataToTree(string[] data)
        {
            var treeRow = 0;
            Tree tree = new Tree();
            foreach (string currentString in data)
            {
                IEnumerable<int> currentListOfNumbersFromString = GetNumbersFromStringFormat(currentString);

                List<TreeElement> rowElements = new List<TreeElement>();
                foreach (int iterator in Enumerable.Range(0, currentListOfNumbersFromString.Count()))
                {
                    var newNumber = new TreeElement(currentListOfNumbersFromString.ElementAt(iterator), tree);
                    rowElements.Add(newNumber);
                }
                tree.InsertTreeRow(treeRow, rowElements);
                treeRow++;
            }
            return tree;
        }

        private IEnumerable<int> GetNumbersFromStringFormat(string firstString)
        {
            var listOfStringNumbers = firstString.Split(' ');
            var listOfNumbers = listOfStringNumbers.Select(s => int.Parse(s));
            return listOfNumbers;
        }

        private class TreeElement
        {
            public override string ToString()
            {
                return string.Format("Element: {0}. LeftChild = {1}. RightChild = {2}", elementValue, LeftChild.elementValue, RightChild.elementValue);
            }
            private int elementValue;
            private int maximumSubtotal;
            private bool subTotalCalculated = false;
            private Tree tree;

            public TreeElement Parent { get; internal set; }
            public TreeElement LeftChild { get; internal set; }
            public TreeElement RightChild { get; internal set; }

            public int MaximumSubTotal
            {
                get
                {
                    if (subTotalCalculated)
                    {
                        return maximumSubtotal;
                    }
                    maximumSubtotal = FindMaximumSubTotal();
                    subTotalCalculated = true;
                    return maximumSubtotal;
                }
            }

            public TreeElement(int elementValue, Tree parentTree)
            {
                this.elementValue = elementValue;
                this.tree = parentTree;
            }

            internal int FindMaximumSubTotal()
            {
                tree.PathIterations++;
                var pathValue = this.elementValue;
                var leftPathMaximumValue = 0;
                if (LeftChild != null) leftPathMaximumValue = LeftChild.MaximumSubTotal;
                var rightPathMaximumValue = 0;
                if (RightChild != null) rightPathMaximumValue = RightChild.MaximumSubTotal;
                var maxOfLeftAndRight = Math.Max(leftPathMaximumValue, rightPathMaximumValue);
                return pathValue + maxOfLeftAndRight;
            }
        }

        private class Tree
        {
            public long PathIterations;
            public override string ToString()
            {
                return string.Format("Rows: {0}. Iterations: {1}", treeRows.Count, this.PathIterations);
            }
            Dictionary<int, List<TreeElement>> treeRows;
            public Tree()
            {
                treeRows = new Dictionary<int, List<TreeElement>>();
            }

            internal void InsertTreeRow(int treeRow, List<TreeElement> rowElements)
            {
                treeRows.Add(treeRow, rowElements);
            }

            internal void FindChildren()
            {
                List<TreeElement> previousRow = null;

                foreach (var rowData in treeRows)
                {
                    var rowNumber = rowData.Key;
                    var currentRow = rowData.Value;

                    if (previousRow == null) //Special case for the first row. Just record it and continue with next row
                    {
                        previousRow = currentRow;
                        continue;
                    }

                    //Normal case for finding children. Previous row is already stored in previousRow variable.

                    int posInRow = 0;
                    //foreach (var rowElement in rowElements)
                    foreach (var rowElement in previousRow)
                    {
                        rowElement.LeftChild = currentRow[posInRow];
                        rowElement.RightChild = currentRow[posInRow + 1];
                        posInRow++;
                    }
                    previousRow = currentRow;
                }
            }

            internal int FindMaximumTotal()
            {
                var topElement = this.treeRows[0].ElementAt(0);
                return topElement.MaximumSubTotal;
            }
        }
    }
}
