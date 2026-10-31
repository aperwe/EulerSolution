using System.Diagnostics;

namespace EulerProblems.Problems.AdditionalProblems;

public enum PersistenceRunState { Running, Completed, Cancelled, Failed }

/// <summary>CompletedChunks excludes partial chunks; LargestEvaluated may include them.</summary>
public sealed record PersistenceRunProgress(
    PersistenceRunState State, TimeSpan Elapsed, ulong? LargestEvaluated,
    ulong? CompletedThrough, PersistenceSearchResult CompletedChunks,
    PersistenceSearchResult Contiguous);

public static partial class MultiplicativePersistenceSearch
{
    /// <summary>
    /// Runs a finite search off the caller's thread. Reports immediately, approximately
    /// every minute, and after all workers stop. Callbacks are serialized, outside locks,
    /// and may run on a background thread. Cancellation throws OperationCanceledException
    /// after the final snapshot; worker failures propagate after cleanup.
    /// </summary>
    public static Task<PersistenceSearchResult> SearchParallelAsync(
        ulong start, ulong count, int workerCount, ulong chunkSize,
        Action<PersistenceRunProgress>? report = null, CancellationToken cancellationToken = default,
        TimeSpan? reportInterval = null)
        => RunAsync(start, count, workerCount, chunkSize, report, cancellationToken,
            reportInterval ?? TimeSpan.FromSeconds(60), Calculate,
            (interval, token) => Task.Delay(interval, token));

    // Test seams keep timing and failure tests finite and deterministic.
    internal static async Task<PersistenceSearchResult> RunAsync(
        ulong start, ulong count, int workerCount, ulong chunkSize,
        Action<PersistenceRunProgress>? report, CancellationToken cancellationToken,
        TimeSpan interval, Func<ulong, int> calculate,
        Func<TimeSpan, CancellationToken, Task> delay)
    {
        if (workerCount < 1) throw new ArgumentOutOfRangeException(nameof(workerCount));
        if (chunkSize == 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));
        if (interval <= TimeSpan.Zero || interval.TotalMilliseconds > uint.MaxValue - 1)
            throw new ArgumentOutOfRangeException(nameof(interval));
        if (count != 0 && count - 1 > ulong.MaxValue - start)
            throw new ArgumentOutOfRangeException(nameof(count));

        using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var tracker = new PersistenceCompletionTracker();
        var watch = Stopwatch.StartNew();
        // Each slot has one writer. A slot lock publishes a coherent ulong value,
        // including on 32-bit runtimes, without a lock shared by all candidates.
        var slots = Enumerable.Range(0, workerCount).Select(_ => new EvaluatedSlot()).ToArray();
        void Report(PersistenceRunState state)
        {
            ulong? largest = null;
            var snapshot = tracker.Snapshot();
            foreach (var slot in slots)
            {
                lock (slot)
                    if (slot.Value is ulong value && (largest is null || value > largest)) largest = value;
            }
            ulong? through = snapshot.Contiguous.EvaluatedCount == 0 ? null
                : start + (snapshot.Contiguous.EvaluatedCount - 1);
            report?.Invoke(new(state, watch.Elapsed, largest, through, snapshot.Overall, snapshot.Contiguous));
        }

        Report(PersistenceRunState.Running);
        var workers = Task.Run(() =>
        {
            ulong allocated = 0;
            stop.Token.ThrowIfCancellationRequested();
            while (allocated < count)
            {
                stop.Token.ThrowIfCancellationRequested();
                var window = AllocateWindow(start, count, workerCount, chunkSize, ref allocated);
                Parallel.For(0, window.Count, new ParallelOptions {
                    MaxDegreeOfParallelism = workerCount, CancellationToken = stop.Token }, index =>
                {
                    try
                    {
                        var chunk = window[index];
                        int best = -1;
                        ulong bestNumber = chunk.Start;
                        for (ulong offset = 0; offset < chunk.Count; offset++)
                        {
                            stop.Token.ThrowIfCancellationRequested();
                            ulong number = chunk.Start + offset;
                            int value = calculate(number);
                            lock (slots[index]) slots[index].Value = number;
                            if (value > best) { best = value; bestNumber = number; }
                        }
                        tracker.Complete(chunk, new(chunk.Count, bestNumber, best));
                    }
                    catch { stop.Cancel(); throw; }
                });
            }
            return tracker.Snapshot().Overall;
        });

        try
        {
            while (!workers.IsCompleted)
            {
                using var tickStop = CancellationTokenSource.CreateLinkedTokenSource(stop.Token);
                var tick = delay(interval, tickStop.Token);
                if (await Task.WhenAny(workers, tick).ConfigureAwait(false) == workers)
                {
                    tickStop.Cancel();
                    try { await tick.ConfigureAwait(false); }
                    catch (OperationCanceledException) when (tickStop.IsCancellationRequested) { }
                    break;
                }
                try { await tick.ConfigureAwait(false); }
                catch (OperationCanceledException) when (stop.IsCancellationRequested) { break; }
                if (!workers.IsCompleted) Report(PersistenceRunState.Running);
            }
            var result = await workers.ConfigureAwait(false);
            Report(PersistenceRunState.Completed);
            return result;
        }
        catch (Exception error)
        {
            stop.Cancel();
            // Always join workers, including if the reporter itself failed.
            try { await workers.ConfigureAwait(false); } catch { }
            var state = error is OperationCanceledException && cancellationToken.IsCancellationRequested
                ? PersistenceRunState.Cancelled : PersistenceRunState.Failed;
            // Preserve the original error if the final callback also fails.
            try { Report(state); } catch { }
            throw;
        }
    }

    private sealed class EvaluatedSlot { internal ulong? Value; }
}
