# Working instructions for AI coding assistants

## Issue-driven development

- All planned repository changes must be tracked by a GitHub issue, including bug fixes, features, enhancements, refactorings, tests, documentation, and configuration.
- Work only on an issue explicitly selected by the user. If no issue has been selected, ask for its number before modifying repository files.
- Read the selected issue and its acceptance criteria before implementation. If GitHub access is unavailable, ask the user to provide the issue contents.
- If the acceptance criteria are missing or unclear, propose them and agree on the scope with the user before implementation.
- Read-only investigation and discussion may take place before an issue is selected.

## Traceability

- Include the issue number in the branch name, for example `fix/42-null-solver`.
- Reference the issue in every commit message, for example `Handle missing solver types safely (#42)`.
- Link each pull request to its issue and describe the changes and validation performed.
- Use `Closes #42` in a pull request only when it fully resolves that issue and should close it on merge.

## Scope and review

- Keep changes focused on the selected issue.
- Report unrelated findings as proposed backlog items. Do not implement them or create additional issues unless the user requests it.
- Explain the implementation plan before editing.
- Run the relevant build and tests where possible. Report the results and any checks that could not be completed.
- Do not commit, push, or merge unless the user explicitly requests those actions.

## Testing policy

These rules apply to local work and to any CI/CD pipeline. CI work is tracked in #117.

- Every solver must have an automated test. `SolverAnswerTests.EverySolverHasATest` fails if a solver has none.
- Run the whole test suite for every commit. Do not filter out, skip, or ignore solver tests to save time. The `Slow` test category is not a reason to leave tests out.
- A solver's time limit is a maximum, and it must be at least 3 minutes. A solver that finishes sooner ends its test sooner.
- Let a solver that never finishes on its own run for 3 minutes, then force-stop it and check its progress and saved results.
- Tests must not read or change real user data, such as persistence checkpoints. Use temporary folders.
- Check the number of tests that ran, not only the exit code. `dotnet test` can exit with 0 when no tests ran.
- Known exception: Windows Smart App Control can block `IntuitionLibraryTests.dll` on the development machine, so its 8 tests do not run. The cause is unknown. Until there is a solution, this does not block other work. Retry those tests, report whether they ran, and do not work around the block. While it lasts, put tests for changes to library behaviour in `EulerProblemsTests`.
- CI must run on a Windows runner, because the projects target `net8.0-windows`. Its job time limit must allow the full suite to finish.

## Learning support

- The user is returning to C# development after several years.
- Explain unfamiliar terminology, important design choices, and the purpose of commands.
- Prefer small, understandable changes that the user can review.