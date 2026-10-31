using System.Text.Json;
using System.Text.Json.Nodes;
using EulerProblems.Storage;

namespace EulerProblems.Problems.AdditionalProblems;

/// <summary>Solver-specific validation over the shared, solver-agnostic record store.</summary>
public sealed class PersistenceCheckpointStore : IDisposable
{
    public const string NormalKey = "additional-001/normal";
    public const string BoundedKey = "additional-001/bounded";
    private readonly CheckpointRecordStore records;
    public string CheckpointPath => records.FilePath;
    public string BackupPath => records.BackupPath;
    public static string DefaultDirectory => CheckpointRecordStore.DefaultDirectory;

    public PersistenceCheckpointStore(string? directory = null, string recordKey = NormalKey)
        => records = new CheckpointRecordStore(recordKey, directory);

    public PersistenceCheckpointLoad? Load()
    {
        var node = records.Read();
        return node == null ? null : new(Decode(node), false);
    }

    public void Save(PersistenceCheckpoint checkpoint)
    {
        ArgumentNullException.ThrowIfNull(checkpoint);
        checkpoint.Validate();
        records.Update(node =>
        {
            if (node != null)
            {
                var previous = Decode(node);
                if (previous.Start != checkpoint.Start || previous.Count != checkpoint.Count ||
                    previous.Completed.EvaluatedCount > checkpoint.Completed.EvaluatedCount)
                    throw new InvalidDataException("Checkpoint range changed or completed coverage moved backwards.");
            }
            return Encode(checkpoint);
        });
    }

    public string ArchiveForFreshStart() => records.ArchiveAndReset();

    private static JsonNode Encode(PersistenceCheckpoint checkpoint) => new JsonObject {
        ["payloadVersion"] = 1,
        ["savedAtUtc"] = JsonSerializer.SerializeToNode(checkpoint.SavedAtUtc),
        ["data"] = JsonSerializer.SerializeToNode(checkpoint)
    };

    private static PersistenceCheckpoint Decode(JsonNode node)
    {
        try
        {
            if (node is not JsonObject record || record["payloadVersion"] is not JsonValue value ||
                !value.TryGetValue<int>(out int version)) throw new InvalidDataException("Missing payload version.");
            if (version != 1) throw new NotSupportedException("Unsupported persistence payload version.");
            var checkpoint = record["data"]?.Deserialize<PersistenceCheckpoint>()
                ?? throw new InvalidDataException("Missing persistence payload.");
            checkpoint.Validate();
            return checkpoint;
        }
        catch (JsonException error) { throw new InvalidDataException("Invalid persistence payload.", error); }
    }

    public void Dispose() => records.Dispose();
}
