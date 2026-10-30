using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("Persistence problem", "Additional 001",
@"Multiplication number persistence
Based on Numberfile article on number persistence: https://www.youtube.com/watch?v=Wim9WJeDTHQ&ab_channel=Numberphile
for number n calculate how many steps before it reaches 0 or single digit following this 
let n = 3456
Step1: n = pers(n) => 3 * 4 * 5 * 6 => 360
Step2: n = pers(n) => 3 * 6 * 0 => 0
END
return number of steps(2).
")]
    public class AdditionalProblem001 : AbstractEulerProblem, IManagedProblem
    {
        private readonly string? checkpointDirectory;
        public AdditionalProblem001() { }
        internal AdditionalProblem001(string checkpointDirectory) => this.checkpointDirectory = checkpointDirectory;

        private static string KeyFor(bool boundedRun) => boundedRun
            ? PersistenceCheckpointStore.BoundedKey : PersistenceCheckpointStore.NormalKey;

        Task IManagedProblem.RunAsync(bool boundedRun, CancellationToken cancellationToken)
            => RunAsync(cancellationToken, checkpointDirectory, count: boundedRun ? 1000UL : DefaultCount,
                recordKey: KeyFor(boundedRun));

        public Task<string> ArchiveProgressAsync(bool boundedRun) => Task.Run(() =>
        {
            using var store = new PersistenceCheckpointStore(checkpointDirectory, KeyFor(boundedRun));
            string archive = store.ArchiveForFreshStart();
            return $"Old checkpoints archived to {archive}. The next run starts at {DefaultStart}.";
        });

        public const ulong DefaultStart = 277857000000000;
        public const ulong DefaultCount = ulong.MaxValue - DefaultStart + 1;

        public Task<PersistenceSearchResult> RunAsync(CancellationToken cancellationToken,
            string? directory = null, ulong start = DefaultStart, ulong count = DefaultCount,
            int? workerCount = null, string recordKey = PersistenceCheckpointStore.NormalKey)
        {
            int workers = workerCount ?? Math.Max(1, Environment.ProcessorCount - 1);
            Answer = $"Long-running persistence search starting at {start}. Loading checkpoint...";
            return MultiplicativePersistenceSearch.SearchResumableAsync(start, count, workers, 1_000_000,
                progress => Answer = FormatProgress(progress, start, workers), cancellationToken, directory,
                recordKey: recordKey);
        }

        public static string FormatProgress(ResumablePersistenceProgress progress, ulong start, int workers)
        {
            var live = progress.Live;
            var saved = progress.Saved;
            string savedThrough = saved == null || saved.Completed.EvaluatedCount == 0
                ? "none" : (saved.Start + (saved.Completed.EvaluatedCount - 1)).ToString();
            static string Best(PersistenceSearchResult? result) => result?.BestNumber is ulong number
                ? $"{number} (persistence {result.BestPersistence})" : "none";
            return $"{live.State} — long-running persistence search; workers: {workers}\n" +
                $"Verified coverage begins at {start}; earlier numbers are outside this run.\n" +
                $"Elapsed: {live.Elapsed}\nLargest evaluated: {live.LargestEvaluated?.ToString() ?? "none"}\n" +
                $"Completed through: {live.CompletedThrough?.ToString() ?? "none"}\nSaved through: {savedThrough}\n" +
                $"Live best (completed chunks): {Best(live.CompletedChunks)}\nSaved best: {Best(saved?.Completed)}\n" +
                $"Last successful save (UTC): {saved?.SavedAtUtc.ToString("u") ?? "none"}\n" +
                $"Checkpoint: {progress.CheckpointPath}\n" +
                (progress.RecoveredFromBackup ? "Recovered from backup; some work may be repeated.\n" : "") +
                "Progress and saves occur approximately every 60 seconds. Stop saves completed coverage.";
        }

        protected override void Solve(out string answer)
        {
            var result = RunAsync(CancellationToken.None).GetAwaiter().GetResult();
            answer = $"Search complete. Best: {result.BestNumber}, persistence: {result.BestPersistence}.";
        }
    }
}
