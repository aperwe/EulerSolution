using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class PersistenceSearchRunTests
{
    [TestMethod]
    public async Task ReportsInitialAndFinalAndMatchesBaseline()
    {
        var reports = new List<PersistenceRunProgress>();
        var result = await MultiplicativePersistenceSearch.SearchParallelAsync(0, 1003, 4, 37, reports.Add);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(0, 1003), result);
        Assert.AreEqual(PersistenceRunState.Running, reports[0].State);
        Assert.IsNull(reports[0].LargestEvaluated);
        Assert.AreEqual(PersistenceRunState.Completed, reports[^1].State);
        Assert.AreEqual(1002UL, reports[^1].CompletedThrough);
        Assert.AreEqual(1002UL, reports[^1].LargestEvaluated);
        Assert.AreEqual(result, reports[^1].Contiguous);
    }

    [TestMethod]
    public async Task CancellationKeepsPartialChunkOutOfCompletedCoverage()
    {
        using var stop = new CancellationTokenSource();
        var reports = new List<PersistenceRunProgress>();
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            MultiplicativePersistenceSearch.RunAsync(0, 10, 1, 5, reports.Add, stop.Token,
                TimeSpan.FromMinutes(1), n => { if (n == 2) stop.Cancel(); return 0; }, Task.Delay));
        Assert.AreEqual(PersistenceRunState.Cancelled, reports[^1].State);
        Assert.AreEqual(2UL, reports[^1].LargestEvaluated);
        Assert.IsNull(reports[^1].CompletedThrough);
        Assert.AreEqual(0UL, reports[^1].CompletedChunks.EvaluatedCount);
    }

    [TestMethod]
    public async Task AlreadyCancelledRunDoesNotEvaluateAnything()
    {
        using var stop = new CancellationTokenSource();
        stop.Cancel();
        var reports = new List<PersistenceRunProgress>();
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            MultiplicativePersistenceSearch.SearchParallelAsync(0, 10, 2, 3, reports.Add, stop.Token));
        Assert.AreEqual(PersistenceRunState.Cancelled, reports[^1].State);
        Assert.IsNull(reports[^1].LargestEvaluated);
    }

    [TestMethod]
    public async Task WorkerFailurePropagatesAfterFinalReport()
    {
        var reports = new List<PersistenceRunProgress>();
        var error = await Assert.ThrowsExceptionAsync<AggregateException>(() =>
            MultiplicativePersistenceSearch.RunAsync(0, 10, 2, 5, reports.Add, default,
                TimeSpan.FromMinutes(1), _ => throw new InvalidOperationException("test failure"), Task.Delay));
        Assert.IsTrue(error.Flatten().InnerExceptions.Any(e => e.Message == "test failure"));
        Assert.AreEqual(PersistenceRunState.Failed, reports[^1].State);
        Assert.AreEqual(0UL, reports[^1].CompletedChunks.EvaluatedCount);
    }

    [TestMethod]
    [Timeout(10000)]
    public async Task ControlledTickReportsDuringWorkAndTimerIsStopped()
    {
        using var release = new ManualResetEventSlim();
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var tick = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var reports = new List<PersistenceRunProgress>();
        int delays = 0;
        int cancelledDelays = 0;
        var run = MultiplicativePersistenceSearch.RunAsync(0, 1, 1, 1, p => {
            reports.Add(p);
            if (reports.Count == 2) release.Set();
        }, default, TimeSpan.FromSeconds(60), _ => {
            entered.TrySetResult();
            if (!release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
            return 0;
        }, (duration, token) => {
            Assert.AreEqual(TimeSpan.FromSeconds(60), duration);
            if (Interlocked.Increment(ref delays) == 1) return tick.Task;
            token.Register(() => Interlocked.Increment(ref cancelledDelays));
            return Task.Delay(Timeout.InfiniteTimeSpan, token);
        });
        await entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
        tick.SetResult();
        await run;
        Assert.AreEqual(PersistenceRunState.Running, reports[1].State);
        Assert.AreEqual(PersistenceRunState.Completed, reports[^1].State);
        Assert.AreEqual(delays - 1, cancelledDelays);
    }

    [TestMethod]
    public async Task ReporterFailureIsPropagated()
    {
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
            MultiplicativePersistenceSearch.SearchParallelAsync(0, 10, 2, 2,
                _ => throw new InvalidOperationException("report failure")));
    }

    [TestMethod]
    [Timeout(10000)]
    public async Task ReporterFailureDuringWorkJoinsWorkers()
    {
        using var release = new ManualResetEventSlim();
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        int active = 0;
        int reports = 0;
        var run = MultiplicativePersistenceSearch.RunAsync(0, 10, 1, 10, _ => {
            if (++reports == 2)
            {
                release.Set();
                throw new InvalidOperationException("report failure");
            }
        }, default, TimeSpan.FromSeconds(60), _ => {
            Interlocked.Increment(ref active);
            try
            {
                entered.TrySetResult();
                if (!release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
                return 0;
            }
            finally { Interlocked.Decrement(ref active); }
        }, (_, _) => entered.Task);
        var error = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => run);
        Assert.AreEqual("report failure", error.Message);
        Assert.AreEqual(0, active);
    }
}
