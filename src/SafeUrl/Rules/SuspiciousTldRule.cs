using SafeUrl.Models;

namespace SafeUrl.Rules;

/// <summary>
/// Rule that checks for suspicious top-level domains commonly used in phishing.
/// </summary>
public class SuspiciousTldRule : ISafetyRule
{
    public string RuleId => "SUSPICIOUS_TLD";
    public string Name => "Suspicious TLD Check";
    public string Description => "Checks for top-level domains commonly associated with malicious sites";

    /// <summary>
    /// List of TLDs that are commonly abused for phishing/malware.
    /// </summary>
    private static readonly HashSet<string> SuspiciousTlds = new(StringComparer.OrdinalIgnoreCase)
    {
        ".tk", ".ml", ".ga", ".cf", ".gq",  // Free TLDs often abused
        ".xyz", ".top", ".work", ".click",   // Cheap TLDs often used for spam
        ".zip", ".mov"                        // Confusing TLDs that look like file extensions
    };

    // TODO: [CONTRIBUTOR] Add more suspicious TLDs based on threat intelligence
    // TODO: [CONTRIBUTOR] Add option to load TLDs from external configuration file

    public IEnumerable<SafetyIssue> Check(string url)
    {
        var issues = new List<SafetyIssue>();

        if (string.IsNullOrWhiteSpace(url))
        {
            return issues;
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            var host = uri.Host.ToLowerInvariant();

            foreach (var tld in SuspiciousTlds)
            {
                if (host.EndsWith(tld, StringComparison.OrdinalIgnoreCase))
                {
                    issues.Add(new SafetyIssue
                    {
                        RuleId = RuleId,
                        Description = $"URL uses suspicious TLD '{tld}' commonly associated with malicious sites",
                        Severity = IssueSeverity.Medium,
                        AffectedUrl = url
                    });
                    break;
                }
            }
        }

        return issues;
    }

    public Task<IEnumerable<SafetyIssue>> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Check(url));
    }
}
