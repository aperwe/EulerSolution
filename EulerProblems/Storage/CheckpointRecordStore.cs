using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EulerProblems.Storage;

public sealed class CheckpointRecoveryRequiredException : IOException
{
    public string DirectoryPath { get; }
    public CheckpointRecoveryRequiredException(string directory, Exception? inner = null)
        : base("The shared checkpoint document is missing or invalid. Explicit backup recovery is required; all solver records may roll back.", inner)
        => DirectoryPath = directory;
}

/// <summary>One solver/run lease, with short cross-process read-modify-write transactions.</summary>
public sealed class CheckpointRecordStore : IDisposable
{
    private readonly string directory;
    private readonly string key;
    private readonly FileStream maintenanceLease;
    private readonly FileStream ownerLease;
    private readonly object gate = new();
    private bool disposed;
    public static string DefaultDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EulerSolution", "runstate");
    public string FilePath => Path.Combine(directory, "checkpoints.json");
    public string BackupPath => FilePath + ".bak";

    public CheckpointRecordStore(string key, string? directory = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        this.key = key;
        this.directory = Path.GetFullPath(directory ?? DefaultDirectory);
        Directory.CreateDirectory(this.directory);
        // Recovery requires exclusive access to this file, excluding every live run.
        string maintenance = Path.Combine(this.directory, "maintenance.lock");
        using (new FileStream(maintenance, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)) { }
        maintenanceLease = new FileStream(maintenance, FileMode.Open, FileAccess.Read, FileShare.Read);
        try
        {
            string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
            ownerLease = new FileStream(Path.Combine(this.directory, hash + ".lock"),
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }
        catch { maintenanceLease.Dispose(); throw; }
    }

    public JsonNode? Read() => Transaction(records => records[key]?.DeepClone(), false);

    /// <summary>Validate and replace only this record while holding the transaction lock.</summary>
    public void Update(Func<JsonNode?, JsonNode> update) => Transaction(records =>
    {
        records[key] = update(records[key]?.DeepClone());
        return true;
    }, true);

    /// <summary>Archives and removes only the selected solver record.</summary>
    public string ArchiveAndReset() => Transaction(records =>
    {
        string archive = Path.Combine(directory, "archive-" + Guid.NewGuid().ToString("N") + ".json");
        WriteNew(archive, new JsonObject { [key] = records[key]?.DeepClone() });
        records.Remove(key);
        return archive;
    }, true);

    private T Transaction<T>(Func<JsonObject, T> operation, bool write)
    {
        lock (gate)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            using var transaction = AcquireTransaction(directory);
            JsonObject document;
            try { document = ReadDocument(FilePath); }
            catch (FileNotFoundException)
            {
                if (File.Exists(BackupPath)) throw new CheckpointRecoveryRequiredException(directory);
                document = new JsonObject { ["formatVersion"] = 1, ["records"] = new JsonObject() };
            }
            catch (InvalidDataException error) { throw new CheckpointRecoveryRequiredException(directory, error); }
            T result = operation((JsonObject)document["records"]!);
            if (write) Replace(directory, document, true);
            return result;
        }
    }

    /// <summary>Call only after explicit user confirmation. No solver may own a record.</summary>
    public static void RestoreBackup(string directory, bool confirmed)
    {
        if (!confirmed) throw new InvalidOperationException("Shared recovery requires confirmation.");
        directory = Path.GetFullPath(directory);
        using var maintenance = new FileStream(Path.Combine(directory, "maintenance.lock"),
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        using var transaction = AcquireTransaction(directory);
        string path = Path.Combine(directory, "checkpoints.json");
        var backup = ReadDocument(path + ".bak");
        if (File.Exists(path)) File.Copy(path, Path.Combine(directory, "before-recovery-" + Guid.NewGuid().ToString("N") + ".json"));
        Replace(directory, backup, false);
    }

    private static FileStream AcquireTransaction(string directory)
    {
        var timeout = Stopwatch.StartNew();
        while (true)
        {
            try { return new FileStream(Path.Combine(directory, "transaction.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None); }
            catch (IOException error) when ((error.HResult & 0xffff) is 32 or 33 && timeout.Elapsed < TimeSpan.FromSeconds(10))
            { Thread.Sleep(10); }
        }
    }

    private static JsonObject ReadDocument(string path)
    {
        try
        {
            var doc = JsonNode.Parse(File.ReadAllText(path)) as JsonObject
                ?? throw new InvalidDataException("Invalid shared checkpoint document.");
            if (doc["formatVersion"] is not JsonValue version || !version.TryGetValue<int>(out int number))
                throw new InvalidDataException("Missing document version.");
            if (number != 1) throw new NotSupportedException("Unsupported shared checkpoint format.");
            if (doc["records"] is not JsonObject) throw new InvalidDataException("Missing records object.");
            return doc;
        }
        catch (JsonException error) { throw new InvalidDataException("Malformed shared checkpoint JSON.", error); }
    }

    private static void WriteNew(string path, JsonNode data)
    {
        using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        JsonSerializer.Serialize(stream, data);
        stream.Flush(true);
    }

    private static void Replace(string directory, JsonObject document, bool keepBackup)
    {
        string path = Path.Combine(directory, "checkpoints.json");
        string temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            WriteNew(temp, document);
            if (File.Exists(path)) File.Replace(temp, path, keepBackup ? path + ".bak" : null);
            else File.Move(temp, path);
        }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }

    public void Dispose()
    {
        lock (gate)
        {
            if (disposed) return;
            ownerLease.Dispose();
            maintenanceLease.Dispose();
            disposed = true;
        }
    }
}
