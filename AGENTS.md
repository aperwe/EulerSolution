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

## Learning support

- The user is returning to C# development after several years.
- Explain unfamiliar terminology, important design choices, and the purpose of commands.
- Prefer small, understandable changes that the user can review.