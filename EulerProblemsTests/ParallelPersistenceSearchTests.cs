using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class ParallelPersistenceSearchTests
{
    [TestMethod]
    public void WindowsCoverEveryCandidateOnceAndBoundOutstandingWork()
    {
        const ulong count = 103;
        ulong start = ulong.MaxValue - count + 1;
        ulong allocated = 0;
        var visited = new HashSet<ulong>();
        while (allocated < count)
        {
            var window = MultiplicativePersistenceSearch.AllocateWindow(start, count, 3, 7, ref allocated);
            Assert.IsTrue(window.Count <= 3);
            foreach (var chunk in window)
            {
                Assert.IsTrue(chunk.Count > 0 && chunk.Count <= 7);
                Assert.AreEqual(start + chunk.Offset, chunk.Start);
                for (ulong i = 0; i < chunk.Count; i++)
                    Assert.IsTrue(visited.Add(checked(chunk.Start + i)), "Duplicate candidate");
            }
        }
        Assert.AreEqual((int)count, visited.Count);
        for (ulong i = 0; i < count; i++) Assert.IsTrue(visited.Contains(start + i));
    }

    [DataTestMethod]
    [DataRow(1, 1UL)]
    [DataRow(2, 37UL)]
    [DataRow(4, 101UL)]
    [DataRow(8, 10000UL)]
    public void ParallelMatchesSequential(int workers, ulong chunkSize)
    {
        foreach (var (start, count) in new[] {
            (0UL, 1003UL), (277857000000000UL, 1001UL),
            (ulong.MaxValue - 100, 101UL), (ulong.MaxValue, 1UL), (ulong.MaxValue, 0UL) })
        {
            Assert.AreEqual(MultiplicativePersistenceSearch.Search(start, count),
                MultiplicativePersistenceSearch.SearchParallel(start, count, workers, chunkSize));
        }
    }

    [TestMethod]
    public void LaterCompletionCannotAdvancePrefixOrItsBest()
    {
        var tracker = new PersistenceCompletionTracker();
        tracker.Complete(new(2, 12, 2), new(2, 13, 2));
        var snapshot = tracker.Snapshot();
        Assert.AreEqual(new PersistenceSearchResult(2, 13, 2), snapshot.Overall);
        Assert.AreEqual(new PersistenceSearchResult(0, null, null), snapshot.Contiguous);
        tracker.Complete(new(0, 10, 2), new(2, 10, 1));
        Assert.AreEqual(new PersistenceSearchResult(4, 13, 2), tracker.Snapshot().Contiguous);
    }

    [TestMethod]
    public void ConcurrentReverseCompletionsKeepSmallestTieAndExactCount()
    {
        var tracker = new PersistenceCompletionTracker();
        Parallel.For(0, 100, i => {
            ulong offset = (ulong)(99 - i);
            tracker.Complete(new(offset, offset, 1), new(1, offset, 0));
        });
        Assert.AreEqual(new PersistenceSearchResult(100, 0, 0), tracker.Snapshot().Overall);
        Assert.AreEqual(tracker.Snapshot().Overall, tracker.Snapshot().Contiguous);
    }

    [TestMethod]
    public void InvalidParametersAreRejectedBeforeSearching()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => MultiplicativePersistenceSearch.SearchParallel(0, 1, 0, 1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => MultiplicativePersistenceSearch.SearchParallel(0, 1, 1, 0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => MultiplicativePersistenceSearch.SearchParallel(ulong.MaxValue, 2, 1, 1));
    }
}
