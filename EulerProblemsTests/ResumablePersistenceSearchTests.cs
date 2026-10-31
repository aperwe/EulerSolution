using System.Text.Json;
using EulerProblems.Storage;
using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class ResumablePersistenceSearchTests
{
    private string directory = null!;
    [TestInitialize]
    public void Setup() => directory = Path.Combine(Path.GetTempPath(), "EulerResumeTests", Guid.NewGuid().ToString("N"));
    [TestCleanup]
    public void Cleanup() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }

    [TestMethod]
    public async Task CancellationSavesPrefixAndResumeRepeatsPartialChunk()
    {
        using var stop = new CancellationTokenSource();
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => Run(0, 100, n => {
            if (n == 7) stop.Cancel();
            return MultiplicativePersistenceSearch.Calculate(n);
        }, stop.Token));
        using (var store = new PersistenceCheckpointStore(directory))
            Assert.AreEqual(5UL, store.Load()!.Checkpoint.Completed.EvaluatedCount);
        var visited = new List<ulong>();
        var result = await Run(0, 100, n => { visited.Add(n); return MultiplicativePersistenceSearch.Calculate(n); });
        Assert.AreEqual(5UL, visited[0]);
        Assert.AreEqual(95, visited.Count);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(0, 100), result);
    }

    [TestMethod]
    public async Task CompletedUpperBoundaryDoesNotWrapOrRecompute()
    {
        await Run(ulong.MaxValue - 4, 5, MultiplicativePersistenceSearch.Calculate);
        var reports = new List<ResumablePersistenceProgress>();
        var result = await Run(ulong.MaxValue - 4, 5, _ => throw new AssertFailedException("Already complete"), report: reports.Add);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(ulong.MaxValue - 4, 5), result);
        Assert.AreEqual(ulong.MaxValue, reports[^1].Live.CompletedThrough);
        Assert.AreEqual(result, reports[^1].Saved!.Completed);
    }

    [TestMethod]
    public async Task MismatchedRangeIsRejected()
    {
        await Run(0, 10, MultiplicativePersistenceSearch.Calculate);
        await Assert.ThrowsExceptionAsync<InvalidDataException>(() => Run(0, 11, MultiplicativePersistenceSearch.Calculate));
    }

    [TestMethod]
    [Timeout(10000)]
    public async Task LaterCompletedChunkIsNotSavedPastUnfinishedGap()
    {
        using var stop = new CancellationTokenSource();
        using var release = new ManualResetEventSlim();
        bool sawGap = false;
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            MultiplicativePersistenceSearch.RunResumableAsync(0, 10, 2, 5, p => {
                if (p.Live.State == PersistenceRunState.Running && p.Live.CompletedChunks.EvaluatedCount == 5)
                {
                    sawGap = true;
                    Assert.IsNull(p.Live.CompletedThrough);
                    Assert.AreEqual(0UL, p.Saved!.Completed.EvaluatedCount);
                    stop.Cancel();
                    release.Set();
                }
            }, stop.Token, directory, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(10), n => {
                if (n == 0 && !release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
                return MultiplicativePersistenceSearch.Calculate(n);
            }, Task.Delay));
        Assert.IsTrue(sawGap);
        using (var store = new PersistenceCheckpointStore(directory))
            Assert.AreEqual(0UL, store.Load()!.Checkpoint.Completed.EvaluatedCount);
        var visited = new List<ulong>();
        var result = await Run(0, 10, n => { visited.Add(n); return MultiplicativePersistenceSearch.Calculate(n); });
        Assert.AreEqual(10, visited.Count);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(0, 10), result);
    }

    [TestMethod]
    public async Task SaveFailureOnCancellationIsNotHidden()
    {
        using var stop = new CancellationTokenSource();
        await Assert.ThrowsExceptionAsync<IOException>(() => Run(0, 10, n => {
            // A directory at the destination prevents checkpoint replacement.
            Directory.CreateDirectory(Path.Combine(directory, "checkpoints.json"));
            stop.Cancel();
            return 0;
        }, stop.Token));
        using var store = new PersistenceCheckpointStore(directory); // ownership released
    }

    [TestMethod]
    [Timeout(10000)]
    public async Task PeriodicSaveOccursWithoutPeriodicUiReport()
    {
        using var release = new ManualResetEventSlim();
        int runningReports = 0;
        string path = Path.Combine(directory, "checkpoints.json");
        var result = await MultiplicativePersistenceSearch.RunResumableAsync(0, 10, 1, 5,
            p => { if (p.Live.State == PersistenceRunState.Running) runningReports++; }, default,
            directory, TimeSpan.FromHours(1), TimeSpan.FromMilliseconds(10), n => {
                if (n == 5 && !release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
                return MultiplicativePersistenceSearch.Calculate(n);
            }, async (interval, token) => {
                if (File.Exists(path))
                {
                    var saved = JsonDocument.Parse(File.ReadAllText(path)).RootElement.GetProperty("records").GetProperty(PersistenceCheckpointStore.NormalKey).GetProperty("data").Deserialize<PersistenceCheckpoint>();
                    if (saved!.Completed.EvaluatedCount == 5) release.Set();
                }
                await Task.Delay(interval, token);
            });
        Assert.IsTrue(release.IsSet);
        Assert.AreEqual(1, runningReports);
        Assert.AreEqual(10UL, result.EvaluatedCount);
    }

    [TestMethod]
    public async Task BackupRecoveryIsReportedAndRecomputedCorrectly()
    {
        using (var store = new PersistenceCheckpointStore(directory))
        {
            var checkpoint = new PersistenceCheckpoint {
                FormatVersion = 1, AlgorithmVersion = PersistenceCheckpoint.CurrentAlgorithmVersion,
                Start = 0, Count = 20, Completed = MultiplicativePersistenceSearch.Search(0, 5),
                SavedAtUtc = DateTimeOffset.UtcNow };
            store.Save(checkpoint);
            store.Save(checkpoint with { Completed = MultiplicativePersistenceSearch.Search(0, 10) });
            File.WriteAllText(store.CheckpointPath, "broken");
        }
        var reports = new List<ResumablePersistenceProgress>();
        CheckpointRecordStore.RestoreBackup(directory, true);
        var result = await Run(0, 20, MultiplicativePersistenceSearch.Calculate, report: reports.Add);
        Assert.IsFalse(reports[0].RecoveredFromBackup);
        Assert.AreEqual(5UL, reports[0].Saved!.Completed.EvaluatedCount);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(0, 20), result);
    }

    private Task<PersistenceSearchResult> Run(ulong start, ulong count, Func<ulong, int> calculate,
        CancellationToken token = default, Action<ResumablePersistenceProgress>? report = null)
        => MultiplicativePersistenceSearch.RunResumableAsync(start, count, 1, 5, report, token,
            directory, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), calculate, Task.Delay);
}
