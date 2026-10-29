namespace EulerProblems.Problems.AdditionalProblems;

internal readonly record struct PersistenceChunk(ulong Offset, ulong Start, ulong Count);

internal sealed record PersistenceCompletionSnapshot(
    PersistenceSearchResult Overall, PersistenceSearchResult Contiguous);

/// <summary>
/// Combines completed chunks. Offset is relative to the search start, so even a
/// range ending at ulong.MaxValue needs no unrepresentable end-exclusive number.
/// Only the contiguous result is suitable for a future restart checkpoint.
/// </summary>
internal sealed class PersistenceCompletionTracker
{
    private readonly object gate = new();
    private readonly Dictionary<ulong, PersistenceSearchResult> pending = new();
    private PersistenceSearchResult overall = new(0, null, null);
    private PersistenceSearchResult contiguous = new(0, null, null);

    internal void Complete(PersistenceChunk chunk, PersistenceSearchResult result)
    {
        lock (gate)
        {
            if (chunk.Count == 0 || result.EvaluatedCount != chunk.Count)
                throw new ArgumentException("Only fully completed nonempty chunks can be recorded.");
            if (chunk.Offset < contiguous.EvaluatedCount || pending.ContainsKey(chunk.Offset))
                throw new InvalidOperationException("Chunk was already completed.");
            pending.Add(chunk.Offset, result);
            overall = Merge(overall, result);
            while (pending.Remove(contiguous.EvaluatedCount, out var next))
                contiguous = Merge(contiguous, next);
        }
    }

    internal PersistenceCompletionSnapshot Snapshot()
    {
        lock (gate) return new(overall, contiguous);
    }

    private static PersistenceSearchResult Merge(PersistenceSearchResult left, PersistenceSearchResult right)
    {
        bool useRight = left.BestPersistence is null ||
            right.BestPersistence > left.BestPersistence ||
            (right.BestPersistence == left.BestPersistence && right.BestNumber < left.BestNumber);
        return new(checked(left.EvaluatedCount + right.EvaluatedCount),
            useRight ? right.BestNumber : left.BestNumber,
            useRight ? right.BestPersistence : left.BestPersistence);
    }
}
