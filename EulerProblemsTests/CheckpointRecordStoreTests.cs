using System.Text.Json.Nodes;
using EulerProblems.Storage;
using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class CheckpointRecordStoreTests
{
    private string directory = null!;
    [TestInitialize]
    public void Setup() => directory = Path.Combine(Path.GetTempPath(), "EulerSharedTests", Guid.NewGuid().ToString("N"));
    [TestCleanup]
    public void Cleanup() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }

    [TestMethod]
    public void ArbitraryRecordsSurviveUpdatesAndResetOfAnotherSolver()
    {
        using var first = new CheckpointRecordStore("solver-a/normal", directory);
        using var second = new CheckpointRecordStore("solver-b/normal", directory);
        second.Update(_ => JsonNode.Parse("""{"futureVersion":99,"arbitrary":[1,"two",null]}""")!);
        string before = second.Read()!.ToJsonString();
        first.Update(_ => JsonValue.Create(42)!);
        Assert.AreEqual(before, second.Read()!.ToJsonString());
        string archive = first.ArchiveAndReset();
        Assert.IsTrue(File.Exists(archive));
        Assert.IsNull(first.Read());
        Assert.AreEqual(before, second.Read()!.ToJsonString());
    }

    [TestMethod]
    public async Task ConcurrentWritersDoNotLoseOtherRecords()
    {
        var tasks = Enumerable.Range(0, 8).Select(i => Task.Run(() => {
            using var store = new CheckpointRecordStore($"solver-{i}", directory);
            for (int n = 0; n < 20; n++) store.Update(_ => JsonValue.Create(n)!);
        }));
        await Task.WhenAll(tasks);
        for (int i = 0; i < 8; i++)
        {
            using var store = new CheckpointRecordStore($"solver-{i}", directory);
            Assert.AreEqual(19, store.Read()!.GetValue<int>());
        }
    }

    [TestMethod]
    public void DuplicateOwnershipFailsButOtherSolversCanRun()
    {
        using var first = new CheckpointRecordStore("a", directory);
        Assert.ThrowsException<IOException>(() => { using var duplicate = new CheckpointRecordStore("a", directory); });
        using var other = new CheckpointRecordStore("b", directory);
        other.Update(_ => JsonValue.Create("ok")!);
    }

    [TestMethod]
    public void MalformedSolverPayloadDoesNotPreventOtherSolverUpdates()
    {
        using (var invalid = new CheckpointRecordStore(PersistenceCheckpointStore.NormalKey, directory))
            invalid.Update(_ => JsonValue.Create("invalid payload")!);
        using var solver = new PersistenceCheckpointStore(directory);
        Assert.ThrowsException<InvalidDataException>(() => solver.Load());
        using var other = new CheckpointRecordStore("other", directory);
        other.Update(_ => JsonValue.Create(123)!);
        Assert.AreEqual(123, other.Read()!.GetValue<int>());
    }

    [TestMethod]
    public void PayloadVersionMismatchIsNotReset()
    {
        using (var raw = new CheckpointRecordStore(PersistenceCheckpointStore.NormalKey, directory))
            raw.Update(_ => JsonNode.Parse("""{"payloadVersion":99,"data":{}}""")!);
        using var store = new PersistenceCheckpointStore(directory);
        Assert.ThrowsException<NotSupportedException>(() => store.Load());
    }

    [TestMethod]
    public void InvalidBackupIsNotRestored()
    {
        using (var store = new CheckpointRecordStore("a", directory))
        {
            store.Update(_ => JsonValue.Create(1)!);
            File.WriteAllText(store.BackupPath, "broken");
        }
        Assert.ThrowsException<InvalidDataException>(() => CheckpointRecordStore.RestoreBackup(directory, true));
        using var intact = new CheckpointRecordStore("a", directory);
        Assert.AreEqual(1, intact.Read()!.GetValue<int>());
    }

    [TestMethod]
    public void RecoveryRequiresAllOwnersToStopAndRestoresWholeDocument()
    {
        using (var first = new CheckpointRecordStore("a", directory))
        using (var second = new CheckpointRecordStore("b", directory))
        {
            first.Update(_ => JsonValue.Create(1)!);
            second.Update(_ => JsonValue.Create(2)!);
            first.Update(_ => JsonValue.Create(3)!); // backup now contains a=1, b=2
            File.WriteAllText(first.FilePath, "broken");
            Assert.ThrowsException<CheckpointRecoveryRequiredException>(() => second.Read());
            Assert.ThrowsException<IOException>(() => CheckpointRecordStore.RestoreBackup(directory, true));
        }
        CheckpointRecordStore.RestoreBackup(directory, true);
        using var restoredA = new CheckpointRecordStore("a", directory);
        using var restoredB = new CheckpointRecordStore("b", directory);
        Assert.AreEqual(1, restoredA.Read()!.GetValue<int>());
        Assert.AreEqual(2, restoredB.Read()!.GetValue<int>());
    }

    [TestMethod]
    public void FailedReplacementPreservesEveryRecord()
    {
        using var first = new CheckpointRecordStore("a", directory);
        using var second = new CheckpointRecordStore("b", directory);
        first.Update(_ => JsonValue.Create(1)!);
        second.Update(_ => JsonValue.Create(2)!);
        string before = File.ReadAllText(first.FilePath);
        using (var held = new FileStream(first.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            Assert.ThrowsException<IOException>(() => first.Update(_ => JsonValue.Create(3)!));
        Assert.AreEqual(before, File.ReadAllText(first.FilePath));
        Assert.AreEqual(0, Directory.GetFiles(directory, "*.tmp").Length);
    }

}
