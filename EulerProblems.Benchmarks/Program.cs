using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.Json;
using EulerProblems.Problems.AdditionalProblems;

RequireRelease();
if (Debugger.IsAttached) throw new InvalidOperationException("Run without the debugger.");
// Explicit finite bounds; this harness never invokes the application's long run or storage.
ulong count = args.Length > 0 ? ulong.Parse(args[0], CultureInfo.InvariantCulture) : 32_000_000;
int repetitions = args.Length > 1 ? int.Parse(args[1], CultureInfo.InvariantCulture) : 3;
string output = args.Length > 2 ? args[2] : "persistence-benchmark.json";
if (count is 0 or > 100_000_000 || repetitions is < 3 or > 9)
    throw new ArgumentOutOfRangeException(nameof(args), "Count must be 1..100,000,000; repetitions must be 3..9.");
ulong chunkSize = args.Length > 3 ? ulong.Parse(args[3], CultureInfo.InvariantCulture) : 200_000;
if (chunkSize == 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));
int[] workers = new[] { 0, 1, 4, 16, Math.Max(1, Environment.ProcessorCount - 1) }.Distinct().ToArray();
ulong[] starts = [0, AdditionalProblem001.DefaultStart];
var rows = new List<Measurement>();
foreach (ulong start in starts)
{
    Console.WriteLine($"Range: start={start}, count={count}, chunk={chunkSize}; warmup...");
    // Full-range warmup for every configuration also checks result equivalence.
    var expected = MultiplicativePersistenceSearch.Search(start, count);
    foreach (int worker in workers) Check(Run(worker));
    var samples = workers.ToDictionary(w => w, _ => new List<double>());
    for (int repeat = 0; repeat < repetitions; repeat++)
    {
        // Rotate execution order to reduce fixed-order thermal/background bias.
        foreach (int worker in workers.Skip(repeat % workers.Length).Concat(workers.Take(repeat % workers.Length)))
        {
            var watch = Stopwatch.StartNew();
            var actual = Run(worker);
            watch.Stop();
            Check(actual);
            samples[worker].Add(watch.Elapsed.TotalSeconds);
            Console.WriteLine($"  workers={worker} sample={repeat + 1} seconds={watch.Elapsed.TotalSeconds:F4}");
        }
    }
    double baseline = Median(samples[0]);
    foreach (int worker in workers)
    {
        double median = Median(samples[worker]);
        rows.Add(new(start, count, worker, samples[worker].ToArray(), median,
            count / median, baseline / median, expected));
    }

    PersistenceSearchResult Run(int worker) => worker == 0
        ? MultiplicativePersistenceSearch.Search(start, count)
        : MultiplicativePersistenceSearch.SearchParallel(start, count, worker, chunkSize);
    void Check(PersistenceSearchResult actual)
    {
        if (actual != expected) throw new InvalidOperationException($"Result mismatch: {actual} != {expected}");
    }
}
var report = new {
    CapturedAtUtc = DateTimeOffset.UtcNow, Runtime = RuntimeInformation.FrameworkDescription,
    OS = RuntimeInformation.OSDescription, LogicalProcessors = Environment.ProcessorCount,
    ServerGC = GCSettings.IsServerGC, ChunkSize = chunkSize, Repetitions = repetitions,
    Notes = "Workers=0 denotes the original sequential calculation. No UI, checkpoint I/O, or forced garbage collection. Full-range warmup per configuration; rotated measured order.",
    Measurements = rows
};
File.WriteAllText(output, JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine($"Results written to {Path.GetFullPath(output)}");
static double Median(List<double> values)
{
    var sorted = values.Order().ToArray();
    int middle = sorted.Length / 2;
    return sorted.Length % 2 == 0 ? (sorted[middle - 1] + sorted[middle]) / 2 : sorted[middle];
}
static void RequireRelease()
{
#if DEBUG
    throw new InvalidOperationException("Build this benchmark in Release mode.");
#endif
}
record Measurement(ulong Start, ulong Count, int Workers, double[] Seconds, double MedianSeconds,
    double CandidatesPerSecond, double Speedup, PersistenceSearchResult Result);
