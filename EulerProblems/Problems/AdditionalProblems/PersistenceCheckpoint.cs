namespace EulerProblems.Problems.AdditionalProblems;

/// <summary>Only contiguous, fully completed work belongs in this record.</summary>
public sealed record PersistenceCheckpoint
{
    public required int FormatVersion { get; init; }
    public required string AlgorithmVersion { get; init; }
    public required ulong Start { get; init; }
    public required ulong Count { get; init; }
    public required PersistenceSearchResult Completed { get; init; }
    public required DateTimeOffset SavedAtUtc { get; init; }

    public const int CurrentFormatVersion = 1;
    public const string CurrentAlgorithmVersion = "decimal-multiplicative-persistence-v1";

    public void Validate()
    {
        if (FormatVersion != CurrentFormatVersion || AlgorithmVersion != CurrentAlgorithmVersion)
            throw new NotSupportedException("Unsupported persistence checkpoint version.");
        if (Count != 0 && Count - 1 > ulong.MaxValue - Start)
            throw new InvalidDataException("Checkpoint range exceeds ulong.MaxValue.");
        if (Completed is null || Completed.EvaluatedCount > Count || SavedAtUtc == default)
            throw new InvalidDataException("Invalid checkpoint completion data or timestamp.");
        if (Completed.EvaluatedCount == 0)
        {
            if (Completed.BestNumber != null || Completed.BestPersistence != null)
                throw new InvalidDataException("Empty coverage cannot have a best result.");
        }
        else if (Completed.BestNumber is not ulong number || number < Start ||
            number - Start >= Completed.EvaluatedCount || Completed.BestPersistence is not int persistence ||
            persistence != MultiplicativePersistenceSearch.Calculate(number))
        {
            throw new InvalidDataException("Best result is inconsistent with completed coverage.");
        }
    }
}

public sealed record PersistenceCheckpointLoad(PersistenceCheckpoint Checkpoint, bool RecoveredFromBackup);
