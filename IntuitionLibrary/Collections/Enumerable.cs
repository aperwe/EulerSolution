using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// Static class containing methods to manipulate ranges of Int64 values.
    /// </summary>
    public static class Enumerable64
    {
        /// <summary>
        /// Generates a sequence of long integral numbers within a specified range.
        /// </summary>
        /// <param name="start">The value of the first long in the sequence.</param>
        /// <param name="count">The number of sequential longs to generate.</param>
        /// <returns>An IEnumerable&lt;long> in C# that contains a range of sequential long numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">count is less than 0.</exception>
        public static IEnumerable<long> Range(long start, long count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("count", "Count is less than 0.");
            var retVal = new LongEnumerable(start, count);
            return retVal;
        }
        /// <summary>
        /// Generates a sequence of ulong integral numbers within a specified range.
        /// </summary>
        /// <param name="start">The value of the first ulong in the sequence.</param>
        /// <param name="count">The number of sequential longs to generate.</param>
        /// <returns>An IEnumerable&lt;long> in C# that contains a range of sequential long numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException">count is less than 0.</exception>
        public static IEnumerable<ulong> Range(ulong start, ulong count)
        {
            var retVal = new ULongEnumerable(start, count);
            return retVal;
        }
    }
    class LongEnumerable : IEnumerable<long>
    {
        private long start;
        private long count;

        public LongEnumerable(long start, long count)
        {
            this.start = start;
            this.count = count;
        }

        IEnumerator<long> IEnumerable<long>.GetEnumerator()
        {
            var enumerator = new LongEnumerator(start, count);
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumerator = new LongEnumerator(start, count);
            return enumerator;
        }
    }
    class ULongEnumerable : IEnumerable<ulong>
    {
        private ulong start;
        private ulong count;

        public ULongEnumerable(ulong start, ulong count)
        {
            this.start = start;
            this.count = count;
        }

        IEnumerator<ulong> IEnumerable<ulong>.GetEnumerator()
        {
            var enumerator = new ULongEnumerator(start, count);
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumerator = new ULongEnumerator(start, count);
            return enumerator;
        }
    }

    class LongEnumerator : IEnumerator<long>
    {
        private long start;
        private long count;
        private long currentValue;
        private long maxValue;

        public LongEnumerator(long start, long count)
        {
            this.start = start;
            this.count = count;
            this.currentValue = this.start - 1;
            this.maxValue = start + count;
        }

        long IEnumerator<long>.Current => currentValue;

        object IEnumerator.Current => currentValue;

        void IDisposable.Dispose() { } //Nothing to do here

        /// <summary>
        /// Since MoveNext is called first before accessing the first value of the iterator
        /// it is important that Iterator is initially positioned 1 element before the actual start value
        /// so that the iterator produces correct sequence.
        /// </summary>
        /// <returns></returns>
        bool IEnumerator.MoveNext()
        {
            currentValue++;
            return currentValue < maxValue;
        }

        void IEnumerator.Reset()
        {
            currentValue = start - 1;
        }
    }
    class ULongEnumerator : IEnumerator<ulong>
    {
        private ulong start;
        private ulong count;
        private ulong currentValue;
        private ulong maxValue;

        public ULongEnumerator(ulong start, ulong count)
        {
            this.start = start;
            this.count = count;
            this.currentValue = this.start - 1;
            this.maxValue = start + count;
        }

        ulong IEnumerator<ulong>.Current => currentValue;

        object IEnumerator.Current => currentValue;

        void IDisposable.Dispose() { } //Nothing to do here

        /// <summary>
        /// Since MoveNext is called first before accessing the first value of the iterator
        /// it is important that Iterator is initially positioned 1 element before the actual start value
        /// so that the iterator produces correct sequence.
        /// </summary>
        /// <returns></returns>
        bool IEnumerator.MoveNext()
        {
            currentValue++;
            return currentValue < maxValue;
        }

        void IEnumerator.Reset()
        {
            currentValue = start - 1;
        }
    }

}
