namespace EulerProblems;

/// <summary>
/// Optional solver capabilities used by the launcher. Implementations own bounded
/// limits and storage paths, publish answers through their existing answer event,
/// and finish cancellation only after workers and final saves have completed.
/// </summary>
public interface IManagedProblem
{
    Task RunAsync(bool boundedRun, CancellationToken cancellationToken);

    /// <summary>Archives saved progress for the chosen mode and returns a user-facing message.</summary>
    Task<string> ArchiveProgressAsync(bool boundedRun);
}
