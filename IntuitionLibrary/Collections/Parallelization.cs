using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Collections
{
    /// <summary>
    /// Parallellzation helper methods
    /// </summary>
    public static class Parallelization
    {
        /// <summary>
        /// Gets a list of sub-ranges that can be used in parallel loop to iterate over whole range.
        /// Typical use: Call .ForAll() delegate on the returned query to distribute work on multiple processors.
        /// </summary>
        /// <param name="start">The value of the first integer in this sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <param name="partitions">The number of partitions (sub-ranges) into which to divide the whole range produced.</param>
        public static ParallelQuery<IEnumerable<int>> GetParallelRanges(int start, int count, int partitions)
        {
            List<IEnumerable<int>> enumerables = new List<IEnumerable<int>>();
            int partitionSize = (count + 1) / partitions; //Handle odd counts properly
            int end = count + start;
            for (int pos = start, range = partitionSize; pos < end; pos += partitionSize)
            {
                if ((pos + range) > end) //Make sure the last partition is capped to ensure proper total count
                {
                    range = end - pos;
                }
                var item = Enumerable.Range(pos, range);
                enumerables.Add(item);
            }
            var parallels = enumerables.AsParallel();
            return parallels;
        }

    }
}
