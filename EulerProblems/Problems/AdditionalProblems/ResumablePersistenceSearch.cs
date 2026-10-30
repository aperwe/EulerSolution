using System.Diagnostics;

namespace EulerProblems.Problems.AdditionalProblems;

/// <summary>Live coverage and the last successfully written checkpoint are distinct.</summary>
public sealed record ResumablePersistenceProgress(
    PersistenceRunProgress Live, PersistenceCheckpoint? Saved,
    string CheckpointPath, bool RecoveredFromBackup);

public static partial class MultiplicativePersistenceSearch
{
    /// <summary>
    /// Loads and owns the checkpoint for this finite range. Only completed contiguous
    /// chunks are saved; unfinished and unsaved work is repeated after interruption.
    /// A save failure stops the run and is propagated, including during cancellation.
    /// </summary>
    public static Task<PersistenceSearchResult> SearchResumableAsync(
        ulong start, ulong count, int workerCount, ulong chunkSize,
        Action<ResumablePersistenceProgress>? report = null,
        CancellationToken cancellationToken = default, string? directory = null,
        TimeSpan? reportInterval = null, TimeSpan? saveInterval = null)
        => RunResumableAsync(start, count, workerCount, chunkSize, report, cancellationToken,
            directory, reportInterval ?? TimeSpan.FromSeconds(60),
            saveInterval ?? TimeSpan.FromSeconds(60), Calculate, Task.Delay);

    internal static Task<PersistenceSearchResult> RunResumableAsync(
        ulong start, ulong count, int workerCount, ulong chunkSize,
        Action<ResumablePersistenceProgress>? report, CancellationToken cancellationToken,
        string? directory, TimeSpan reportInterval, TimeSpan saveInterval,
        Func<ulong, int> calculate, Func<TimeSpan, CancellationToken, Task> delay)
    {
        if (workerCount < 1) throw new ArgumentOutOfRangeException(nameof(workerCount));
        if (chunkSize == 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));
        foreach (var interval in new[] { reportInterval, saveInterval })
            if (interval <= TimeSpan.Zero || interval.TotalMilliseconds > uint.MaxValue - 1)
                throw new ArgumentOutOfRangeException(nameof(interval));
        if (count != 0 && count - 1 > ulong.MaxValue - start)
            throw new ArgumentOutOfRangeException(nameof(count));

        // File access and the initial callback also stay off the caller's UI thread.
        return Task.Run(async () =>
        {
            using var store = new PersistenceCheckpointStore(directory);
            var loaded = store.Load();
            var saved = loaded?.Checkpoint;
            if (saved != null && (saved.Start != start || saved.Count != count))
                throw new InvalidDataException("Requested range differs from the checkpoint range.");
            var prefix = saved?.Completed ?? new PersistenceSearchResult(0, null, null);
            ulong remaining = count - prefix.EvaluatedCount;
            // Avoid forming MaxValue + 1 when the entire range is already complete.
            ulong resumeStart = remaining == 0 ? start : start + prefix.EvaluatedCount;
            var clock = Stopwatch.StartNew();
            TimeSpan lastSave = TimeSpan.Zero, lastReport = TimeSpan.Zero;
            bool first = true;
            Exception? saveFailure = null;

            void Observe(PersistenceRunProgress progress)
            {
                var contiguous = Combine(prefix, progress.Contiguous);
                var overall = Combine(prefix, progress.CompletedChunks);
                ulong? completedThrough = contiguous.EvaluatedCount == 0 ? null
                    : start + (contiguous.EvaluatedCount - 1);
                ulong? largest = progress.LargestEvaluated;
                if (largest == null && prefix.EvaluatedCount > 0)
                    largest = start + (prefix.EvaluatedCount - 1);
                var live = progress with { Contiguous = contiguous, CompletedChunks = overall,
                    CompletedThrough = completedThrough, LargestEvaluated = largest };
                bool terminal = progress.State != PersistenceRunState.Running;
                if (saveFailure == null && (terminal || (!first && clock.Elapsed - lastSave >= saveInterval)))
                {
                    var next = new PersistenceCheckpoint {
                        FormatVersion = PersistenceCheckpoint.CurrentFormatVersion,
                        AlgorithmVersion = PersistenceCheckpoint.CurrentAlgorithmVersion,
                        Start = start, Count = count, Completed = contiguous, SavedAtUtc = DateTimeOffset.UtcNow };
                    try { store.Save(next); }
                    catch (Exception error) { saveFailure = error; throw; }
                    saved = next; // Publish saved coverage only after successful replacement.
                    lastSave = clock.Elapsed;
                }
                if (first || terminal || clock.Elapsed - lastReport >= reportInterval)
                {
                    first = false;
                    lastReport = clock.Elapsed;
                    report?.Invoke(new(live, saved, store.CheckpointPath, loaded?.RecoveredFromBackup == true));
                }
            }

            try
            {
                var result = await RunAsync(resumeStart, remaining, workerCount, chunkSize,
                    Observe, cancellationToken,
                    reportInterval < saveInterval ? reportInterval : saveInterval,
                    calculate, delay).ConfigureAwait(false);
                return Combine(prefix, result);
            }
            catch (Exception error) when (saveFailure != null)
            {
                throw new IOException("Search stopped because its checkpoint could not be saved.",
                    ReferenceEquals(error, saveFailure) ? error : new AggregateException(error, saveFailure));
            }
        });
    }

    private static PersistenceSearchResult Combine(PersistenceSearchResult prefix, PersistenceSearchResult suffix)
    {
        if (suffix.EvaluatedCount == 0) return prefix;
        bool useSuffix = prefix.BestPersistence == null || suffix.BestPersistence > prefix.BestPersistence ||
            (suffix.BestPersistence == prefix.BestPersistence && suffix.BestNumber < prefix.BestNumber);
        return new(checked(prefix.EvaluatedCount + suffix.EvaluatedCount),
            useSuffix ? suffix.BestNumber : prefix.BestNumber,
            useSuffix ? suffix.BestPersistence : prefix.BestPersistence);
    }
}
