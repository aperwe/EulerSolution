# Additional 001: running, saving, and validating the search

## Using the application

Open `EulerSolution.sln`, select `EulerStarter` as the startup project, and run it.
Select **Additional 001**, choose the mode, then click **Run**.

- **Bounded run** evaluates 1,000 numbers beginning at `277857000000000`.
  It uses the `additional-001/bounded` record. This is a functional check, not a
  performance benchmark: the range is smaller than one worker chunk.
- Normal mode starts at the same value and searches through `ulong.MaxValue`.
  It is intentionally very long-running. Earlier numbers are not verified by this run.
- The normal record is `additional-001/normal`. Running again resumes its saved
  prefix. An already completed bounded record returns without recomputing.
- **Stop** requests cooperative cancellation. Partial chunks are not marked complete.
  Workers stop, completed contiguous coverage is saved, and a final report is shown.
- Closing the window during a managed run requests the same cancellation and save.
  A failure keeps the window open so the error can be read.
- **Fresh start** asks for confirmation, archives only the selected mode's record,
  and removes that record. It does not reset another solver or start computation.

The launcher uses `IManagedProblem`, not a concrete solver type. Existing solvers
without that interface run through the original synchronous entry point on a
background task; bounded, stop, and fresh-start capabilities do not apply to them.

## What the progress values mean

The application reports immediately, approximately every 60 seconds, and at the end.
Saving has its own interval, also approximately 60 seconds. These are not real-time
deadlines: scheduling, disk access, and a busy UI can delay them.

| Field | Meaning |
| --- | --- |
| Largest evaluated | Highest candidate observed, possibly inside an unfinished chunk. |
| Completed through | Every number from the run's start through this boundary has completed. |
| Saved through | Contiguous boundary in the last successfully written checkpoint. |
| Live best | Best result from completed chunks, including chunks beyond an unfinished gap. |
| Saved best | Best result within the saved contiguous prefix. |

The live best does not include results from partial chunks. If a later chunk finishes
first, neither its boundary nor its best result can be used to skip the earlier gap.
After an interruption, unsaved or partial work is repeated. A forced process stop
cannot perform a final save; only the previous successful checkpoint is available.

Workers receive distinct chunks of 200,000 numbers (reduced from 1,000,000 after review). At most one window of chunks
(up to the worker count) is outstanding. The next window waits for the slowest chunk.
This bounds memory and speculative work but can leave workers idle. The default is
`max(1, Environment.ProcessorCount - 1)` workers. A worker is a task, not a CPU pinned
to a particular core; logical processors do not guarantee proportional speedup.

## Shared persistence and recovery

The current file is `%LOCALAPPDATA%\EulerSolution\runstate\checkpoints.json`.
Stable solver/mode keys identify separate payloads in its `records` object.
The file format and solver payload are versioned independently.

Each run holds a per-record ownership lock. Short cross-process transactions reread
the document, modify only that record, flush a temporary file, then replace the
document. Different records may be owned by different processes. A failed save
stops the search and must not be treated as successful saved coverage.

`checkpoints.json.bak` contains the preceding whole-document generation, not an
independent backup for each solver. If the document is missing with a backup present,
or is corrupt, the UI offers explicit recovery. **All solver records may roll back.**
Stop all other owners before confirming. Recovery archives the current file first.
Declining leaves the file unchanged. Unsupported document versions are rejected.
An invalid individual payload does not invalidate other solver records.

Record resets create `archive-*.json`; recovery creates `before-recovery-*.json`.
Lock files may remain after a run: ownership is the open file handle, not the file's
existence. Do not delete lock files to bypass ownership. Migration from the former
problem-specific checkpoint files has been removed following the completed migration.

## Repeatable bounded Release benchmark

From the repository root, with competing CPU-intensive applications stopped:

```powershell
dotnet build EulerSolution.sln -c Release
dotnet run --project EulerProblems.Benchmarks/EulerProblems.Benchmarks.csproj -c Release --no-build -- 32000000 3 docs/persistence-benchmark-200k.json 200000
dotnet test EulerProblemsTests/EulerProblemsTests.csproj -c Release --no-build --no-restore
```

The benchmark's arguments are candidate count **per range**, repetitions, output
JSON path, and optional chunk size (default 200,000). Use a separate output path
for each comparison so prior measurements are preserved. It refuses Debug builds and an attached debugger. Counts are restricted
to 1–100 million and repetitions to 3–9. It never invokes normal application mode,
the UI, or checkpoint storage. Its only output file is the requested measurement JSON.

It measures ranges starting at zero and at the application's configured starting
point. Every configuration receives a full-range warmup. Measured order rotates
between repetitions to reduce fixed-order bias. Each result must equal the sequential
baseline, including candidate count, maximum persistence, and smallest tied number.
The independent arithmetic-reference tests provide separate correctness evidence.

Worker count zero in the report means the sequential baseline, retaining the original
string/LINQ digit calculation. Other configurations use the same calculation through
the parallel lifecycle with 1, 4, 16, and the default worker count (duplicates removed).
The report includes individual durations, median seconds, candidates/second, and
speedup relative to the sequential median. No forced garbage collection is used.

This measures computation and scheduling overhead, not WPF responsiveness, checkpoint
I/O latency, or long-run thermal behavior. Results depend on digit patterns, available
cores, garbage collection, background load, and power settings. A short bounded UI
run is not evidence of parallel performance. Changes to worker count or chunk size
should be justified by comparable repeated measurements, not CPU utilization alone.

## Measurements recorded on 2026-10-31

Machine: AMD Ryzen 9 9950X3D, 16 physical cores / 32 logical processors.
Runtime: .NET 8.0.33; SDK 8.0.427; Windows build 10.0.26300; workstation GC.
Release build, no debugger, 32 million candidates per range, 1-million chunks,
three measured repetitions following full-range warmups. Background load and power
settings were not controlled; these are local observations, not hardware guarantees.
The code under measurement was the search implementation at commit 1773881.
Raw samples and exact results are in [persistence-benchmark.json](persistence-benchmark.json).

| Range start | Configuration | Median seconds | Million candidates/s | Speedup |
| --- | --- | ---: | ---: | ---: |
| 0 | Sequential | 4.054 | 7.89 | 1.00x |
| 0 | 1 worker | 4.193 | 7.63 | 0.97x |
| 0 | 4 workers | 2.103 | 15.21 | 1.93x |
| 0 | 16 workers | 0.836 | 38.27 | 4.85x |
| 0 | 31 workers (application default) | 1.063 | 30.09 | 3.81x |
| 277857000000000 | Sequential | 2.783 | 11.50 | 1.00x |
| 277857000000000 | 1 worker | 3.137 | 10.20 | 0.89x |
| 277857000000000 | 4 workers | 1.399 | 22.87 | 1.99x |
| 277857000000000 | 16 workers | 0.635 | 50.42 | 4.39x |
| 277857000000000 | 31 workers (application default) | 0.787 | 40.68 | 3.54x |

Every configuration matched the sequential result. From zero, the smallest best
candidate was 26888999 with persistence 9. Near the starting point, it was
277857000000000 with persistence 1. All numbers in that second interval retain a
zero digit, so they exercise the zero shortcut and are not representative of every
future interval. The zero-based interval exercises more varied persistence lengths.

The one-worker path shows lifecycle/cancellation/progress-tracking overhead relative
to the plain sequential loop. Sixteen workers were fastest here, but 32 chunks form
one 31-chunk window plus a single-chunk tail at 31 workers, versus two full windows
at 16. That finite-window effect, sample variance, and uncontrolled background load
prevent a confident general conclusion about the best worker count. No production
worker-count, chunk-size, or algorithm tuning was made in this increment. Longer
balanced comparisons and ranges without fixed zero digits would be needed before
changing the agreed defaults.

Validation: all 60 bounded regression tests passed in Release mode. Both Release
and Debug solution builds succeeded; compilation of existing source reported 189
warnings. No application search or real checkpoint file was used by the benchmark.

## Manual verification still outstanding

Automated bounded tests cover save/resume, gap handling, cancellation, storage failure,
record isolation, and legacy solver lifecycle. They do not verify the complete WPF
interaction. Reviewers should check selection/control state, final status after Stop,
window closure, and recovery confirmation (Yes/No/error) in a disposable test environment.
Do not corrupt the live shared checkpoint to exercise a dialog. The 1,000-number mode
usually finishes too quickly to test Stop or the minute cadence interactively.


## Follow-up: 200,000-number chunks

The application chunk size was reduced to 200,000 at the user's request. The
original measurements and raw JSON above are retained unchanged. The new raw
samples are in [persistence-benchmark-200k.json](persistence-benchmark-200k.json).
This run used the same machine, Release configuration, ranges, 32-million candidate
count, worker counts, warmups, and three repetitions. Every result again matched
the sequential baseline. Negative elapsed change below means a shorter run.

| Range start | Workers | 1M median s | 200K median s | 200K M candidates/s | Elapsed change | Speedup: 1M → 200K |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| 0 | Sequential | 4.054 | 3.575 | 8.95 | -11.8% | 1.00x → 1.00x |
| 0 | 1 | 4.193 | 3.785 | 8.45 | -9.7% | 0.97x → 0.94x |
| 0 | 4 | 2.103 | 1.524 | 21.00 | -27.6% | 1.93x → 2.35x |
| 0 | 16 | 0.836 | 0.780 | 41.04 | -6.8% | 4.85x → 4.59x |
| 0 | 31 | 1.063 | 0.949 | 33.74 | -10.8% | 3.81x → 3.77x |
| 277857000000000 | Sequential | 2.783 | 2.441 | 13.11 | -12.3% | 1.00x → 1.00x |
| 277857000000000 | 1 | 3.137 | 2.759 | 11.60 | -12.1% | 0.89x → 0.88x |
| 277857000000000 | 4 | 1.399 | 1.167 | 27.42 | -16.6% | 1.99x → 2.09x |
| 277857000000000 | 16 | 0.635 | 0.639 | 50.06 | 0.7% | 4.39x → 3.82x |
| 277857000000000 | 31 | 0.787 | 0.713 | 44.87 | -9.3% | 3.54x → 3.42x |


### Interpretation and recommended setting

- At the application's 31-worker default, 200K chunks reduced observed elapsed time
  by roughly 11% from zero and 9% near the configured start. However, sequential
  times also fell by roughly 12% in both ranges. Baseline-relative speedups changed
  from 3.81x to 3.77x and from 3.54x to 3.42x, respectively. These separate runs do
  **not** establish a causal throughput improvement from the smaller chunks.
- Sixteen workers remained the fastest tested setting: about 0.780 s and 0.639 s.
  This is a 16-physical-core / 32-logical-processor machine, not a 32-physical-core
  machine. These results do not establish an optimum for other CPUs.
- The chunk handoff count increases fivefold (32 to 160 chunks per range), but
  there is no evidence here that this overhead overwhelms parallel execution.
  The one-worker overhead relative to sequential remains approximately 6% and 13%.
- Window size in *chunks* is unchanged: at most one chunk per worker. At 31 workers,
  outstanding candidate coverage falls from 31 million to 6.2 million. This also
  reduces the amount of work that can lie beyond an unfinished gap. The 1M run
  has a final window of one chunk; the 200K run has a final window of five chunks.
  Both have finite-range tail effects. Cancellation is still checked per candidate,
  so its responsiveness does not rely on shrinking the chunks.
- **Keep 200K as the current practical setting**, as requested: its measured
  throughput is comparable after accounting for baseline variation, with finer
  completion/checkpoint granularity and less outstanding work. Do not call it the
  optimal chunk size. The evidence only compares two sizes on two ranges, one of
  which always contains a zero digit.
- A credible optimum would require interleaved 100K/200K/500K/1M measurements with
  more repetitions, larger balanced ranges, and varied high-digit patterns under
  controlled background load. Worker count should be evaluated independently.
  This follow-up leaves the agreed default worker count unchanged.

Validation after the change: Release solution build succeeded and all 60 bounded
tests passed. Only fixed-range benchmarks were run; production checkpoint data
was not read or modified.
