using SafeUrl.Models;

namespace SafeUrl.Rules;

/// <summary>
/// Rule that checks for excessively long URLs which may indicate obfuscation.
/// </summary>
public class UrlLengthRule : ISafetyRule
{
    public string RuleId => "URL_LENGTH";
    public string Name => "URL Length Check";
    public string Description => "Checks for excessively long URLs that may indicate obfuscation attempts";

    /// <summary>
    /// Maximum URL length before triggering a warning.
    /// </summary>
    public int MaxLength { get; set; } = 100;

    /// <summary>
    /// URL length that triggers a high severity warning.
    /// </summary>
    public int CriticalLength { get; set; } = 200;

    public IEnumerable<SafetyIssue> Check(string url)
    {
        var issues = new List<SafetyIssue>();

        if (string.IsNullOrWhiteSpace(url))
        {
            return issues;
        }

        if (url.Length > CriticalLength)
        {
            issues.Add(new SafetyIssue
            {
                RuleId = RuleId,
                Description = $"URL is excessively long ({url.Length} characters), which may indicate obfuscation",
                Severity = IssueSeverity.High,
                AffectedUrl = url
            });
        }
        else if (url.Length > MaxLength)
        {
            issues.Add(new SafetyIssue
            {
                RuleId = RuleId,
                Description = $"URL is unusually long ({url.Length} characters)",
                Severity = IssueSeverity.Low,
                AffectedUrl = url
            });
        }

        return issues;
    }

    public Task<IEnumerable<SafetyIssue>> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Check(url));
    }
}
