using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// 08 02 22 97 38 15 00 40 00 75 04 05 07 78 52 12 50 77 91 08
    /// 49 49 99 40 17 81 18 57 60 87 17 40 98 43 69 48 04 56 62 00
    /// 81 49 31 73 55 79 14 29 93 71 40 67 53 88 30 03 49 13 36 65
    /// 52 70 95 23 04 60 11 42 69 24 68 56 01 32 56 71 37 02 36 91
    /// 22 31 16 71 51 67 63 89 41 92 36 54 22 40 40 28 66 33 13 80
    /// 24 47 32 60 99 03 45 02 44 75 33 53 78 36 84 20 35 17 12 50
    /// 32 98 81 28 64 23 67 10 26 38 40 67 59 54 70 66 18 38 64 70
    /// 67 26 20 68 02 62 12 20 95 63 94 39 63 08 40 91 66 49 94 21
    /// 24 55 58 05 66 73 99 26 97 17 78 78 96 83 14 88 34 89 63 72
    /// 21 36 23 09 75 00 76 44 20 45 35 14 00 61 33 97 34 31 33 95
    /// 78 17 53 28 22 75 31 67 15 94 03 80 04 62 16 14 09 53 56 92
    /// 16 39 05 42 96 35 31 47 55 58 88 24 00 17 54 24 36 29 85 57
    /// 86 56 00 48 35 71 89 07 05 44 44 37 44 60 21 58 51 54 17 58
    /// 19 80 81 68 05 94 47 69 28 73 92 13 86 52 17 77 04 89 55 40
    /// 04 52 08 83 97 35 99 16 07 97 57 32 16 26 26 79 33 27 98 66
    /// 88 36 68 87 57 62 20 72 03 46 33 67 46 55 12 32 63 93 53 69
    /// 04 42 16 73 38 25 39 11 24 94 72 18 08 46 29 32 40 62 76 36
    /// 20 69 36 41 72 30 23 88 34 62 99 69 82 67 59 85 74 04 36 16
    /// 20 73 35 29 78 31 90 01 74 31 49 71 48 86 81 16 23 57 05 54
    /// 01 70 54 71 83 51 54 69 16 92 33 48 61 43 52 01 89 19 67 48
    /// </summary>
    public class EulerProblem11 : AbstractEulerProblem
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;
            string numbersString = "08 02 22 97 38 15 00 40 00 75 04 05 07 78 52 12 50 77 91 08 49 49 99 40 17 81 18 57 60 87 17 40 98 43 69 48 04 56 62 00 81 49 31 73 55 79 14 29 93 71 40 67 53 88 30 03 49 13 36 65 52 70 95 23 04 60 11 42 69 24 68 56 01 32 56 71 37 02 36 91 22 31 16 71 51 67 63 89 41 92 36 54 22 40 40 28 66 33 13 80 24 47 32 60 99 03 45 02 44 75 33 53 78 36 84 20 35 17 12 50 32 98 81 28 64 23 67 10 26 38 40 67 59 54 70 66 18 38 64 70 67 26 20 68 02 62 12 20 95 63 94 39 63 08 40 91 66 49 94 21 24 55 58 05 66 73 99 26 97 17 78 78 96 83 14 88 34 89 63 72 21 36 23 09 75 00 76 44 20 45 35 14 00 61 33 97 34 31 33 95 78 17 53 28 22 75 31 67 15 94 03 80 04 62 16 14 09 53 56 92 16 39 05 42 96 35 31 47 55 58 88 24 00 17 54 24 36 29 85 57 86 56 00 48 35 71 89 07 05 44 44 37 44 60 21 58 51 54 17 58 19 80 81 68 05 94 47 69 28 73 92 13 86 52 17 77 04 89 55 40 04 52 08 83 97 35 99 16 07 97 57 32 16 26 26 79 33 27 98 66 88 36 68 87 57 62 20 72 03 46 33 67 46 55 12 32 63 93 53 69 04 42 16 73 38 25 39 11 24 94 72 18 08 46 29 32 40 62 76 36 20 69 36 41 72 30 23 88 34 62 99 69 82 67 59 85 74 04 36 16 20 73 35 29 78 31 90 01 74 31 49 71 48 86 81 16 23 57 05 54 01 70 54 71 83 51 54 69 16 92 33 48 61 43 52 01 89 19 67 48";
            var numbers = numbersString.Split(' ').AsParallel().Select((a, b) => int.Parse(a)).ToList();
            ArrayAPI arrayAPI = new ArrayAPI(numbers);
            int maxRight = 0, maxDown = 0, maxDiagLeftDown = 0, maxDiagRightDown = 0;
            int iterationsRight = 0, iterationsDown = 0, iterationsDiagLeftDown = 0, iterationsDiagRightDown = 0;
            int maxX = 20, maxY = 20; //Dimensions of the array

            #region Adjacent to the right
            {
                foreach (int y in Enumerable.Range(0, 20))
                {
                    foreach (int x in Enumerable.Range(0, 16))
                    {
                        var elements = arrayAPI.ElementsAtRight(x, y, 4);
                        var product = elements.Aggregate((a, prod) => a * prod);
                        maxRight = Math.Max(maxRight, product);
                        iterationsRight++;
                    }
                }
            }
            #endregion

            #region Adjacent down
            {
                foreach (int y in Enumerable.Range(0, 16))
                {
                    foreach (int x in Enumerable.Range(0, 20))
                    {
                        var elements = arrayAPI.ElementsDown(x, y, 4);
                        var product = elements.Aggregate((a, prod) => a * prod);
                        maxDown = Math.Max(maxDown, product);
                        iterationsDown++;
                    }
                }
            }
            #endregion

            #region Adjacent diagonally down and left
            {
                foreach (int y in Enumerable.Range(0, 16))
                {
                    foreach (int x in Enumerable.Range(3, 16))
                    {
                        var elements = arrayAPI.ElementsDiagonallyLeftDown(x, y, 4);
                        var product = elements.Aggregate((a, prod) => a * prod);
                        maxDiagLeftDown = Math.Max(maxDiagLeftDown, product);
                        iterationsDiagLeftDown++;
                    }
                }
            }
            #endregion

            #region Adjacent diagonally down and right
            {
                foreach (int y in Enumerable.Range(0, 16))
                {
                    foreach (int x in Enumerable.Range(0, 16))
                    {
                        var elements = arrayAPI.ElementsDiagonallyRightDown(x, y, 4);
                        var product = elements.Aggregate((a, prod) => a * prod);
                        maxDiagRightDown = Math.Max(maxDiagRightDown, product);
                        iterationsDiagRightDown++;
                    }
                }
            }
            #endregion

            var totalMax = Math.Max(maxRight, Math.Max(maxDown, Math.Max(maxDiagLeftDown, maxDiagRightDown)));
            var totalIterations = iterationsRight + iterationsDown + iterationsDiagLeftDown + iterationsDiagRightDown;
            var elapsedTime = DateTime.Now - start;
            Answer = string.Format("Elapsed computation time: {0}. Greatest product of four adjacent numbers in the same direction (up, down, left, right, or diagonally) is {1}. Iterations: {2}.", elapsedTime, totalMax, totalIterations);
        }
        private class ArrayAPI
        {
            private List<int> numbers;
            private int maxX = 20;
            private int maxY = 20;

            public ArrayAPI(List<int> numbers)
            {
                this.numbers = numbers;
            }

            internal int ElementAt(int x, int y)
            {
                return numbers[StartPosition(x, y)];
            }

            private int StartPosition(int x, int y)
            {
                return x + y * maxY;
            }

            internal IEnumerable<int> ElementsAtRight(int x, int y, int count)
            {
                return numbers.Skip(StartPosition(x, y)).Take(count);
            }

            internal IEnumerable<int> ElementsDown(int x, int y, int count)
            {
                List<int> results = new List<int>();
                var start = numbers.Skip(StartPosition(x, y)); results.Add(start.First()); //1
                start = start.Skip(maxX); results.Add(start.First()); //2
                start = start.Skip(maxX); results.Add(start.First()); //3
                start = start.Skip(maxX); results.Add(start.First()); //4
                return results;
            }

            internal IEnumerable<int> ElementsDiagonallyLeftDown(int x, int y, int count)
            {
                List<int> results = new List<int>();
                var start = numbers.Skip(StartPosition(x, y)); results.Add(start.First()); //1
                start = start.Skip(maxX - 1); results.Add(start.First()); //2
                start = start.Skip(maxX - 1); results.Add(start.First()); //3
                start = start.Skip(maxX - 1); results.Add(start.First()); //4
                return results;
            }

            internal IEnumerable<int> ElementsDiagonallyRightDown(int x, int y, int count)
            {
                List<int> results = new List<int>();
                var start = numbers.Skip(StartPosition(x, y)); results.Add(start.First()); //1
                start = start.Skip(maxX + 1); results.Add(start.First()); //2
                start = start.Skip(maxX + 1); results.Add(start.First()); //3
                start = start.Skip(maxX + 1); results.Add(start.First()); //4
                return results;
            }
        }
    }
}
