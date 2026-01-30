# Contributing to safe-url-validator

Thanks for your interest in contributing! We welcome bug reports, feature requests, documentation improvements, and code contributions. This document outlines the recommended workflow and expectations to make contributions smooth and reviewable.

## Table of contents
- How to contribute
- Opening issues
- Submitting pull requests
- Branching & commit guidelines
- Coding style & tests
- Local development and tests
- CI and review process
- License & contributor agreement
- Communication

## How to contribute
There are several ways to contribute:
- Report bugs or request features by opening an issue.
- Fix issues or implement small features via pull requests (PRs).
- Improve or clarify documentation (README, docs).
- Add or improve tests.

If you're unsure where to start, check open issues or open a discussion/issue asking for guidance.

## Opening issues
When opening an issue, please:
- Use a clear, descriptive title.
- Describe the problem or desired feature and why it matters.
- Include a minimal, reproducible example (if applicable).
- Provide environment details (OS, .NET SDK version).
- Label the issue appropriately (bug, enhancement, docs).

Suggested issue template:
- Title: short, descriptive
- Body:
  - Problem / Motivation
  - Steps to reproduce (if bug)
  - Expected behavior
  - Actual behavior
  - Environment (.NET SDK version, OS)

## Submitting pull requests
1. Fork the repository.
2. Create a topic branch from `main`:
   - Branch name format: `username/short-description` or `fix/issue-123-description`
3. Make changes in the branch and commit logically (one concern per commit).
4. Ensure tests pass locally and add tests for new functionality.
5. Push your branch to your fork and open a PR against `main` in this repository.
6. In the PR description, reference any related issues (e.g., `Fixes #123`) and include a short summary of changes and rationale.

PR checklist (for contributors):
- [ ] Code builds and runs locally (`dotnet build`).
- [ ] Tests pass (`dotnet test`).
- [ ] New/changed behavior is covered by tests where feasible.
- [ ] Changes follow the coding style and project conventions.
- [ ] Documentation/README updated if the user-facing behavior changed.

## Branching & commit guidelines
- Base feature branches on `main`.
- Keep changes scoped and small where possible.
- Use descriptive commit messages. We recommend the Conventional Commits style:
  - feat: add new feature
  - fix: bug fix
  - docs: documentation only changes
  - style: formatting, missing semi-colons, etc.
  - refactor: code change that neither fixes a bug nor adds a feature
  - test: adding missing tests or correcting existing tests
  - chore: build process or auxiliary tools

Example commit:
```
feat(validation): add IDN support to URL parser
```

## Coding style & conventions
This repository uses C#/.NET conventions:
- Follow .NET naming and layout conventions (PascalCase for public members, camelCase for private fields).
- Keep lines reasonably short (wrap at ~120 characters).
- Use XML documentation comments for public APIs.
- Prefer immutability where practical.
- Format code using `dotnet format` or an `.editorconfig` if provided.

If you add new dependencies, prefer minimal and actively maintained packages and explain the rationale in the PR.

## Tests
- Unit tests should be added under the test project (or follow the repo's test structure).
- Use `dotnet test` to run tests locally.
- Aim to keep tests deterministic and fast.
- Include test data and examples when helpful.

## Local development
To build and test locally (typical):
1. Install the .NET SDK (match project SDK; check `global.json` if present).
2. Clone the repo:
   ```
   git clone https://github.com/2024si96553-droid/safe-url-validator.git
   cd safe-url-validator
   ```
3. Restore and build:
   ```
   dotnet restore
   dotnet build
   ```
4. Run tests:
   ```
   dotnet test  
   ```

If the project provides scripts or a Makefile, prefer those instructions.

## CI and review
- Contributors should expect a continuous integration (CI) run on PRs (e.g., GitHub Actions). Ensure your branch passes CI checks.
- Maintainters will perform code review; please respond to review comments and push updates to your PR branch.

## License & contributor agreement
By contributing to this project, you agree that your contributions are licensed under the project's license (see LICENSE). If you are contributing from an employer or similar, ensure you have the right to submit the contribution under this license.

## Communication & support
- Use issues for bug reports and feature requests.
- For discussion or help, open an issue with the "discussion" or "help" tag if available.
- Be respectful and follow a professional, welcoming tone.

Thank you for helping improve safe-url-validator!
