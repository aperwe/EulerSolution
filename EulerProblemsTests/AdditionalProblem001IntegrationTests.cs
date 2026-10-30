using EulerProblems.Problems.AdditionalProblems;
using EulerProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class AdditionalProblem001IntegrationTests
{
    private string directory = null!;
    [TestInitialize]
    public void Setup() => directory = Path.Combine(Path.GetTempPath(), "EulerIntegrationTests", Guid.NewGuid().ToString("N"));
    [TestCleanup]
    public void Cleanup() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }

    [TestMethod]
    public async Task ManagedInterfaceOwnsBoundedRangeAndArchiveLocation()
    {
        IManagedProblem managed = new AdditionalProblem001(directory);
        await managed.RunAsync(true, default);
        string boundedDirectory = directory;
        using (var store = new PersistenceCheckpointStore(boundedDirectory, PersistenceCheckpointStore.BoundedKey))
            Assert.AreEqual(1000UL, store.Load()!.Checkpoint.Completed.EvaluatedCount);
        Assert.IsFalse(File.Exists(Path.Combine(directory, "checkpoint.json")));
        StringAssert.Contains(await managed.ArchiveProgressAsync(true), "archived");
        using (var store = new PersistenceCheckpointStore(boundedDirectory, PersistenceCheckpointStore.BoundedKey)) Assert.IsNull(store.Load());
        await managed.RunAsync(true, default);
    }

    [TestMethod]
    public async Task SolverPublishesReadableBoundedResultsAndResumes()
    {
        var solver = new AdditionalProblem001();
        var messages = new List<string>();
        solver.AnswerAvailableEventHandler += (_, e) => messages.Add(e.Answer);
        var result = await solver.RunAsync(default, directory, count: 1000, workerCount: 2);
        Assert.AreEqual(MultiplicativePersistenceSearch.Search(AdditionalProblem001.DefaultStart, 1000), result);
        StringAssert.Contains(messages[0], "Loading checkpoint");
        foreach (var label in new[] { "Completed through:", "Saved through:", "Live best", "Saved best:", "Checkpoint:" })
            StringAssert.Contains(messages[^1], label);
        Assert.AreEqual(result, await solver.RunAsync(default, directory, count: 1000, workerCount: 2));
    }

    [TestMethod]
    public async Task CancelledSolverSavesAndPublishesFinalStatus()
    {
        using var stop = new CancellationTokenSource();
        stop.Cancel();
        var solver = new AdditionalProblem001();
        string last = "";
        solver.AnswerAvailableEventHandler += (_, e) => last = e.Answer;
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => solver.RunAsync(stop.Token, directory, count: 1000));
        StringAssert.Contains(last, "Cancelled");
        using var store = new PersistenceCheckpointStore(directory);
        Assert.AreEqual(0UL, store.Load()!.Checkpoint.Completed.EvaluatedCount);
    }

    [TestMethod]
    public async Task FreshStartArchivesBothFilesAndAllowsNewRun()
    {
        var solver = new AdditionalProblem001();
        await solver.RunAsync(default, directory, count: 10);
        await solver.RunAsync(default, directory, count: 10);
        using (var store = new PersistenceCheckpointStore(directory))
        {
            var previous = store.Load()!.Checkpoint;

            string archive = store.ArchiveForFreshStart();
            Assert.IsTrue(File.Exists(archive));
            StringAssert.Contains(File.ReadAllText(archive), "additional-001/normal");
            Assert.IsNull(store.Load());
        }
        Assert.AreEqual(20UL, (await solver.RunAsync(default, directory, count: 20)).EvaluatedCount);
    }

    [TestMethod]
    public async Task ConflictingOwnerIsReportedWithoutResettingData()
    {
        using var store = new PersistenceCheckpointStore(directory);
        await Assert.ThrowsExceptionAsync<IOException>(() => new AdditionalProblem001().RunAsync(default, directory, count: 10));
        Assert.IsNull(store.Load());
    }
}
