# SafeUrl Validator

SafeUrl Validator is a lightweight .NET library and demo console app that expands and analyzes URLs to determine whether they appear safe. It expands shortened links (when possible) and applies a set of heuristic safety rules (HTTPS presence, suspicious TLDs/domains, URL length, redirect chains, etc.) to produce a structured safety report.

This README replaces the original generic template (Python/Node/Java) with accurate, actionable documentation for the C#/.NET implementation in this repository.

Status
- Language: C#
- Target Framework: .NET 8.0 (repo contains net8.0 artifacts)
- Components: library (src/SafeUrl), demo console app (samples/SafeUrl.Demo), tests (tests/SafeUrl.Tests)
- License: MIT — see LICENSE

Features
- Expands URLs (detects redirect chains and final destination)
- Heuristic safety checks (HTTPS, suspicious TLDs, suspicious domain patterns, URL length)
- Structured results with score, status, and issue list
- Extensible rules: add custom ISafetyRule implementations
- Demo application (interactive and batch modes) for CLI-style usage

Quick links
- Repository: https://github.com/2024si96553-droid/safe-url-validator
- Library entry point: src/SafeUrl/SafeUrlClient.cs
- Demo: samples/SafeUrl.Demo
- Tests: tests/SafeUrl.Tests
- Contributing: CONTRIBUTING.md
- License: LICENSE (MIT)

Requirements
- .NET SDK 8.0 (or matching SDK for your chosen target). If you prefer another version, update the projects to target that framework.
- Internet access if you enable/perform URL expansion or DNS-based checks

Quick start (build, run demo, test)
1. Clone:
   ```bash
   git clone https://github.com/2024si96553-droid/safe-url-validator.git
   cd safe-url-validator
   ```

2. Restore and build:
   ```bash
   dotnet restore
   dotnet build -c Release
   ```

3. Run the demo console app:
   ```bash
   dotnet run --project samples/SafeUrl.Demo -- https://example.com
   ```
   The demo prints expansion details and a safety result (e.g., `SAFE` or `UNSAFE` with reasons).

4. Run tests:
   ```bash
   dotnet test
   ```

Library usage example
The repo exposes a SafeUrlClient with convenience methods for expanding and analyzing URLs.

```csharp
using SafeUrl;

async Task ExampleAsync()
{
    using var client = new SafeUrlClient();

    // Analyze a URL (expands + checks safety)
    var analysis = await client.AnalyzeAsync("https://example.com");

    Console.WriteLine(analysis.IsSafe ? "SAFE" : "UNSAFE");
    Console.WriteLine($"Score: {analysis.SafetyResult.Score}/100");

    foreach (var issue in analysis.SafetyResult.Issues)
    {
        Console.WriteLine($" - [{issue.Severity}] {issue.Description} (url: {issue.AffectedUrl})");
    }
}
```

Key types & API (brief)
- SafeUrlClient
  - AnalyzeAsync(string url, CancellationToken) -> Task<UrlAnalysisResult>
  - Analyze(string url) -> UrlAnalysisResult
  - ExpandAsync / Expand
  - CheckSafetyAsync / CheckSafety
  - AddRule(ISafetyRule) — register custom rule at runtime

- UrlAnalysisResult
  - ExpandedUrl (OriginalUrl, FinalUrl, RedirectCount, ElapsedMilliseconds)
  - SafetyResult (IsSafe / Status, Score, Issues)

- SafetyIssue
  - RuleId, Description, IssueSeverity, AffectedUrl

Configuration & extension
- Rules are implemented under src/SafeUrl/Rules. The SafetyChecker registers a default set of rules (HttpsRule, SuspiciousTldRule, UrlLengthRule, SuspiciousDomainRule).
- Add custom rules by implementing ISafetyRule and calling SafeUrlClient.AddRule(...) or create a custom ISafetyChecker and pass to SafeUrlClient constructor.
- Consider adding a ValidatorOptions/Settings object if you need runtime toggles for DNS lookups, remote threat feeds, or allow/block lists.

CLI & demo
- The repository has samples/SafeUrl.Demo which provides an interactive console demonstrating common library usage and printing human-friendly output.
- For CLI distribution you can:
  - Publish the console app as a dotnet global tool (Pack with <PackAsTool>true</PackAsTool> and push to NuGet).
  - Publish self-contained single-file executables for target runtimes with `dotnet publish -r <RID> --self-contained true /p:PublishSingleFile=true`.

Packaging & publishing (recommended)
1. NuGet (library):
   - Add package metadata to the library csproj (PackageId, Version, Authors, Description).
   - Create a package:
     ```bash
     dotnet pack src/SafeUrl -c Release
     ```
   - Push:
     ```bash
     dotnet nuget push bin/Release/*.nupkg -s https://api.nuget.org/v3/index.json -k <NUGET_API_KEY>
     ```

2. Dotnet global tool (CLI):
   - Make a console project, mark it as a tool and set ToolCommandName in csproj.
   - Pack and publish to NuGet; users install with:
     ```bash
     dotnet tool install --global <ToolPackageId>
     ```

3. Self-contained single-file binaries:
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
   ```

CI recommendations
- Add a GitHub Actions workflow to run:
  - dotnet restore
  - dotnet build --no-restore
  - dotnet test --no-build
- On release/tag:
  - dotnet pack and publish NuGet package (if desired)
  - Build and upload self-contained binaries to GitHub Releases

Example GitHub Actions actions:
- Build + test on push/PR
- Release pipeline on tag to pack/publish artifacts

Examples of expected output
- Demo output (human readable)
  ```
  SAFE
  ```
  or
  ```
  UNSAFE
  Score: 42/100
  Issues:
   - [Medium] URL uses suspicious TLD '.tk'
   - [Low] URL missing HTTPS
  ```

Contributing
Please read CONTRIBUTING.md for full contribution guidance. In short:
- Fork and branch from main.
- Add tests for new behavior.
- Follow Conventional Commits for clear history.
- Open a PR and reference related issues.

Security
- If you plan to enable remote checks (DNS, HTTP expansion), be mindful of network safety (timeouts, rate limits, opt-in toggles).
- Do not log secrets or sensitive data from URLs. Consider safe sanitization for logs or telemetry.

Roadmap / suggestions
- Add ThreatListRule to check public threat feeds (IOC lists).
- Add RedirectChainRule to detect complex redirect patterns/loops.
- Provide JSON output mode in the demo/CLI for automation.
- Publish NuGet package and optional dotnet tool.

License
This project is licensed under the MIT License — see LICENSE.

Contact / Maintainers
Open issues or PRs on the repository: https://github.com/2024si96553-droid/safe-url-validator

Thank you for using and contributing to SafeUrl Validator!
