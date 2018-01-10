using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary>
    /// he following iterative sequence is defined for the set of positive integers:
    /// n → n/2 (n is even)
    /// n → 3n + 1 (n is odd)
    /// 
    /// Using the rule above and starting with 13, we generate the following sequence:
    /// 
    /// 13 → 40 → 20 → 10 → 5 → 16 → 8 → 4 → 2 → 1
    /// It can be seen that this sequence (starting at 13 and finishing at 1) contains 10 terms. Although it has not been proved yet (Collatz Problem), it is thought that all starting numbers finish at 1.
    /// 
    /// Which starting number, under one million, produces the longest chain?
    /// 
    /// NOTE: Once the chain starts the terms are allowed to go above one million.
    /// </summary>
    public class EulerProblem14 : AbstractEulerProblem
    {
        public override void Solve()
        {
            DateTime start = DateTime.Now;

            List<Map> map = new List<Map>();
            ParallelEnumerable.Range(1, 1000000).ForAll(index =>
            {
                Map testedMap = new Map() { Number = index };
                var collatz = testedMap.CalculateCollatzSequence();
                testedMap.CollatzLength = collatz.Count;
                lock(map) map.Add(testedMap);
            });

            var elapsedTime = DateTime.Now - start;
            var longestCollatz = map.OrderBy(m => m.CollatzLength).Last();
            Answer = string.Format("Elapsed computation time: {0}. The number (under one million) that produces the longest chain is: {1}. Chain length: {2}.", elapsedTime,
                longestCollatz.Number, longestCollatz.CollatzLength
                //string.Concat(response.Take(10).Select(a => a.digit.ToString()))
                );


        }
        class Map
        {
            internal int Number;
            internal int CollatzLength;

            internal List<long> CalculateCollatzSequence()
            {
                long current = Number;

                List<long> sequence = new List<long>();
                while (current != 1)
                {
                    sequence.Add(current);
                    if ((current % 2) == 0) //if odd
                    {
                        current = current / 2;
                    }
                    else
                    {
                        current = 3 * current + 1;
                    }

                }
                sequence.Add(current); // this should always be 1
                return sequence;
            }
        }
    }
}
