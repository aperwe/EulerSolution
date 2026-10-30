using System.Text.Json;
using EulerProblems.Problems.AdditionalProblems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EulerProblemsTests;

[TestClass]
public class PersistenceCheckpointStoreTests
{
    private string directory = null!;
    [TestInitialize]
    public void CreateDirectory() => directory = Path.Combine(Path.GetTempPath(), "EulerCheckpointTests", Guid.NewGuid().ToString("N"));
    [TestCleanup]
    public void Cleanup() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }

    private static PersistenceCheckpoint Checkpoint(ulong completed = 10) => new()
    {
        FormatVersion = PersistenceCheckpoint.CurrentFormatVersion,
        AlgorithmVersion = PersistenceCheckpoint.CurrentAlgorithmVersion,
        Start = 0, Count = 100,
        Completed = MultiplicativePersistenceSearch.Search(0, completed),
        SavedAtUtc = DateTimeOffset.UtcNow
    };

    [TestMethod]
    public void RoundTripAndPreviousGenerationBackup()
    {
        using var store = new PersistenceCheckpointStore(directory);
        Assert.IsNull(store.Load());
        var first = Checkpoint();
        store.Save(first);
        var second = Checkpoint(20);
        store.Save(second);
        Assert.AreEqual(new PersistenceCheckpointLoad(second, false), store.Load());
        Assert.AreEqual(first, JsonSerializer.Deserialize<PersistenceCheckpoint>(File.ReadAllText(store.BackupPath)));
        Assert.AreEqual(0, Directory.GetFiles(directory, "*.tmp").Length);
    }

    [TestMethod]
    public void RecoveryPreservesGoodBackupDuringNextSave()
    {
        using var store = new PersistenceCheckpointStore(directory);
        var first = Checkpoint();
        store.Save(first);
        store.Save(Checkpoint(20));
        File.WriteAllText(store.CheckpointPath, "broken");
        Assert.AreEqual(new PersistenceCheckpointLoad(first, true), store.Load());
        store.Save(Checkpoint(30));
        Assert.AreEqual(first, JsonSerializer.Deserialize<PersistenceCheckpoint>(File.ReadAllText(store.BackupPath)));
        Assert.IsFalse(store.Load()!.RecoveredFromBackup);
    }

    [TestMethod]
    public void MissingPrimaryRecoversBackup()
    {
        using var store = new PersistenceCheckpointStore(directory);
        store.Save(Checkpoint());
        store.Save(Checkpoint(20));
        File.Delete(store.CheckpointPath);
        Assert.IsTrue(store.Load()!.RecoveredFromBackup);
    }

    [TestMethod]
    public void CorruptionIsNotSilentlyReset()
    {
        using var store = new PersistenceCheckpointStore(directory);
        File.WriteAllText(store.CheckpointPath, "{}");
        Assert.ThrowsException<InvalidDataException>(() => store.Load());
        Assert.ThrowsException<InvalidDataException>(() => store.Save(Checkpoint()));
        File.WriteAllText(store.BackupPath, "null");
        Assert.ThrowsException<InvalidDataException>(() => store.Load());
        Assert.AreEqual("{}", File.ReadAllText(store.CheckpointPath));
    }

    [TestMethod]
    public void UnsupportedVersionDoesNotFallBackToOlderBackup()
    {
        using var store = new PersistenceCheckpointStore(directory);
        store.Save(Checkpoint());
        store.Save(Checkpoint(20));
        File.WriteAllText(store.CheckpointPath, JsonSerializer.Serialize(Checkpoint() with { FormatVersion = 99 }));
        Assert.ThrowsException<NotSupportedException>(() => store.Load());
    }

    [TestMethod]
    public void ExclusiveOwnershipIsReleasedOnDispose()
    {
        using (var first = new PersistenceCheckpointStore(directory))
            Assert.ThrowsException<IOException>(() => { using var second = new PersistenceCheckpointStore(directory); });
        using var reopened = new PersistenceCheckpointStore(directory);
        reopened.Save(Checkpoint());
    }

    [TestMethod]
    public void ReplacementFailureLeavesPrimaryRecoverable()
    {
        using var store = new PersistenceCheckpointStore(directory);
        var first = Checkpoint();
        store.Save(first);
        // Permit validation reads, but deny deletion/replacement of the destination.
        using (var held = new FileStream(store.CheckpointPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            Assert.ThrowsException<IOException>(() => store.Save(Checkpoint(20)));
        Assert.AreEqual(first, store.Load()!.Checkpoint);
        Assert.AreEqual(0, Directory.GetFiles(directory, "*.tmp").Length);
    }

    [TestMethod]
    public void InvalidCoverageAndBestResultsAreRejected()
    {
        using var store = new PersistenceCheckpointStore(directory);
        foreach (var invalid in new[] {
            Checkpoint() with { Start = ulong.MaxValue },
            Checkpoint() with { Completed = new(101, 0, 0) },
            Checkpoint() with { Completed = new(0, 0, 0) },
            Checkpoint() with { Completed = new(10, 10, 1) },
            Checkpoint() with { Completed = new(10, 0, 9) } })
            Assert.ThrowsException<InvalidDataException>(() => store.Save(invalid));
        Assert.IsNull(store.Load());
    }

    [TestMethod]
    public void EmptyAndUpperBoundaryCheckpointsRoundTrip()
    {
        using var store = new PersistenceCheckpointStore(directory);
        var checkpoint = Checkpoint(0) with { Start = ulong.MaxValue, Count = 1 };
        store.Save(checkpoint);
        checkpoint = checkpoint with { Completed = MultiplicativePersistenceSearch.Search(ulong.MaxValue, 1) };
        store.Save(checkpoint);
        Assert.AreEqual(checkpoint, store.Load()!.Checkpoint);
    }

    [TestMethod]
    public void RangeChangesAndRegressionsAreRejected()
    {
        using var store = new PersistenceCheckpointStore(directory);
        store.Save(Checkpoint(20));
        Assert.ThrowsException<InvalidDataException>(() => store.Save(Checkpoint()));
        Assert.ThrowsException<InvalidDataException>(() => store.Save(Checkpoint(30) with { Count = 200 }));
    }
}
