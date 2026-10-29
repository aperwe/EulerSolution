namespace EulerProblems.Problems.AdditionalProblems;

/// <summary>Sequential, finite baseline for AdditionalProblem001.</summary>
public static class MultiplicativePersistenceSearch
{
    /// <summary>
    /// Searches finite, distinct chunks in parallel. Each window contains at most
    /// workerCount chunks; the next window starts only after this one completes.
    /// This bounds memory and how far computation can advance beyond a gap.
    /// </summary>
    public static PersistenceSearchResult SearchParallel(
        ulong start, ulong count, int workerCount, ulong chunkSize)
    {
        if (workerCount < 1) throw new ArgumentOutOfRangeException(nameof(workerCount));
        if (chunkSize == 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));
        if (count != 0 && count - 1 > ulong.MaxValue - start)
            throw new ArgumentOutOfRangeException(nameof(count), "The range exceeds ulong.MaxValue.");

        var completed = new PersistenceCompletionTracker();
        ulong allocated = 0;
        while (allocated < count)
        {
            var window = AllocateWindow(start, count, workerCount, chunkSize, ref allocated);

            Parallel.ForEach(window, new ParallelOptions { MaxDegreeOfParallelism = workerCount }, chunk =>
            {
                // Calculation and local aggregation need no shared lock.
                var result = Search(chunk.Start, chunk.Count);
                completed.Complete(chunk, result);
            });
        }
        return completed.Snapshot().Overall;
    }

    // Called serially by the coordinator, after the previous window has finished.
    // Inputs have already been validated by SearchParallel.
    internal static List<PersistenceChunk> AllocateWindow(
        ulong start, ulong count, int workerCount, ulong chunkSize, ref ulong allocated)
    {
        var window = new List<PersistenceChunk>();
        for (int i = 0; i < workerCount && allocated < count; i++)
        {
            ulong length = Math.Min(chunkSize, count - allocated);
            window.Add(new PersistenceChunk(allocated, start + allocated, length));
            allocated += length;
        }
        return window;
    }

    /// <summary>
    /// Evaluates exactly count candidates starting at start, including start.
    /// An empty range has no best result. Equal persistences favor the smallest number.
    /// The full ulong domain must be split into ranges because its size exceeds ulong.MaxValue.
    /// </summary>
    public static PersistenceSearchResult Search(ulong start, ulong count)
    {
        if (count == 0)
            return new PersistenceSearchResult(0, null, null);
        if (count - 1 > ulong.MaxValue - start)
            throw new ArgumentOutOfRangeException(nameof(count), "The range exceeds ulong.MaxValue.");

        int bestPersistence = -1;
        ulong bestNumber = start;
        for (ulong offset = 0; offset < count; offset++)
        {
            ulong candidate = start + offset;
            int persistence = Calculate(candidate);
            if (persistence > bestPersistence)
            {
                bestPersistence = persistence;
                bestNumber = candidate;
            }
        }

        return new PersistenceSearchResult(count, bestNumber, bestPersistence);
    }

    /// <summary>Counts digit-multiplication steps until a single digit remains.</summary>
    public static int Calculate(ulong number)
    {
        int steps = 0;
        while (number > 9)
        {
            number = NextPersistenceValue(number);
            steps++;
        }
        return steps;
    }

    // Preserve the original calculation for the sequential baseline. The shortcut
    // to 10 preserves the remaining step count, not the actual digit product.
    private static ulong NextPersistenceValue(ulong number)
    {
        var digits = number.ToString().ToArray().Select(b => b - '0');
        if (digits.Any(b => b == 0)) return 0;
        if (digits.Any(b => b == 5))
        {
            if (digits.Any(b => b == 2)) return 10;
            if (digits.Any(b => b == 4)) return 10;
            if (digits.Any(b => b == 8)) return 10;
        }

        ulong product = 1;
        foreach (var digit in digits) product *= (ulong)digit;
        return product;
    }
}

/// <summary>A completed finite search; both best fields are null for an empty range.</summary>
public sealed record PersistenceSearchResult(ulong EvaluatedCount, ulong? BestNumber, int? BestPersistence);
