using System.Text.Json;

namespace EulerProblems.Problems.AdditionalProblems;

/// <summary>
/// Single-owner checkpoint storage for AdditionalProblem001. No search or UI work
/// is performed here. Keep this store open for the entire run to retain ownership.
/// </summary>
public sealed class PersistenceCheckpointStore : IDisposable
{
    private readonly FileStream ownership;
    private readonly object gate = new();
    private bool disposed;
    public string CheckpointPath { get; }
    public string BackupPath => CheckpointPath + ".bak";
    public static string DefaultDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "EulerSolution", "AdditionalProblem001");

    public PersistenceCheckpointStore(string? directory = null)
    {
        directory = Path.GetFullPath(directory ?? DefaultDirectory);
        Directory.CreateDirectory(directory);
        CheckpointPath = Path.Combine(directory, "checkpoint.json");
        // The lock file is deliberately retained: deleting it could race another owner.
        ownership = new FileStream(CheckpointPath + ".lock", FileMode.OpenOrCreate,
            FileAccess.ReadWrite, FileShare.None);
    }

    public PersistenceCheckpointLoad? Load()
    {
        lock (gate)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            try
            {
                var primary = Read(CheckpointPath);
                if (primary != null) return new(primary, false);
            }
            catch (InvalidDataException primaryError)
            {
                try
                {
                    var backup = Read(BackupPath);
                    if (backup != null) return new(backup, true);
                }
                catch (InvalidDataException backupError)
                {
                    throw new InvalidDataException("Both checkpoint and backup are invalid.",
                        new AggregateException(primaryError, backupError));
                }
                throw;
            }
            var fallback = Read(BackupPath);
            return fallback == null ? null : new(fallback, true);
        }
    }

    public void Save(PersistenceCheckpoint checkpoint)
    {
        lock (gate)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            ArgumentNullException.ThrowIfNull(checkpoint);
            checkpoint.Validate();
            var previous = Load(); // Never silently overwrite invalid/incompatible state.
            if (previous is not null &&
                (previous.Checkpoint.Start != checkpoint.Start || previous.Checkpoint.Count != checkpoint.Count ||
                 previous.Checkpoint.Completed.EvaluatedCount > checkpoint.Completed.EvaluatedCount))
                throw new InvalidDataException("Checkpoint range changed or completed coverage moved backwards.");

            string temporary = CheckpointPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    JsonSerializer.Serialize(stream, checkpoint);
                    stream.Flush(flushToDisk: true);
                }
                if (File.Exists(CheckpointPath))
                {
                    // Do not replace the good backup with the corrupt primary after recovery.
                    File.Replace(temporary, CheckpointPath,
                        previous?.RecoveredFromBackup == true ? null : BackupPath);
                }
                else File.Move(temporary, CheckpointPath);
            }
            finally
            {
                if (File.Exists(temporary)) File.Delete(temporary);
            }
        }
    }

    private static PersistenceCheckpoint? Read(string path)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var checkpoint = JsonSerializer.Deserialize<PersistenceCheckpoint>(stream)
                ?? throw new InvalidDataException("Checkpoint contains null.");
            checkpoint.Validate();
            return checkpoint;
        }
        catch (FileNotFoundException) { return null; }
        catch (JsonException error) { throw new InvalidDataException("Malformed checkpoint JSON.", error); }
    }

    public void Dispose()
    {
        lock (gate)
        {
            if (disposed) return;
            ownership.Dispose();
            disposed = true;
        }
    }
}
