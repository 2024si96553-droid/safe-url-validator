using System.Text.RegularExpressions;
using SafeUrl.Models;

namespace SafeUrl.Rules;

/// <summary>
/// Rule that checks for suspicious domain patterns commonly used in phishing.
/// </summary>
public partial class SuspiciousDomainRule : ISafetyRule
{
    public string RuleId => "SUSPICIOUS_DOMAIN";
    public string Name => "Suspicious Domain Pattern Check";
    public string Description => "Checks for domain patterns commonly associated with phishing";

    public IEnumerable<SafetyIssue> Check(string url)
    {
        var issues = new List<SafetyIssue>();

        if (string.IsNullOrWhiteSpace(url))
        {
            return issues;
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return issues;
        }

        var host = uri.Host.ToLowerInvariant();

        // Check for excessive hyphens (common in phishing domains)
        var hyphenCount = host.Count(c => c == '-');
        if (hyphenCount >= 3)
        {
            issues.Add(new SafetyIssue
            {
                RuleId = RuleId,
                Description = $"Domain contains {hyphenCount} hyphens, which is common in phishing URLs",
                Severity = IssueSeverity.Medium,
                AffectedUrl = url
            });
        }

        // Check for IP address instead of domain name
        if (IpAddressRegex().IsMatch(host))
        {
            issues.Add(new SafetyIssue
            {
                RuleId = RuleId,
                Description = "URL uses IP address instead of domain name, which is suspicious",
                Severity = IssueSeverity.High,
                AffectedUrl = url
            });
        }

        // Check for suspicious subdomains that mimic legitimate sites
        var suspiciousKeywords = new[] { "login", "signin", "secure", "account", "verify", "update", "confirm" };
        foreach (var keyword in suspiciousKeywords)
        {
            if (host.Contains(keyword) && !IsKnownLegitDomain(host))
            {
                issues.Add(new SafetyIssue
                {
                    RuleId = RuleId,
                    Description = $"Domain contains '{keyword}' which may indicate a phishing attempt",
                    Severity = IssueSeverity.Medium,
                    AffectedUrl = url
                });
                break;
            }
        }

        return issues;
    }

    public Task<IEnumerable<SafetyIssue>> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Check(url));
    }

    /// <summary>
    /// Checks if the domain is a known legitimate domain.
    /// </summary>
    private static bool IsKnownLegitDomain(string host)
    {
        // TODO: [CONTRIBUTOR] Expand this list of known legitimate domains
        var legitDomains = new[]
        {
            "google.com", "microsoft.com", "apple.com", "amazon.com",
            "github.com", "facebook.com", "twitter.com", "linkedin.com"
        };

        return legitDomains.Any(d => host.EndsWith(d, StringComparison.OrdinalIgnoreCase));
    }

    [GeneratedRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")]
    private static partial Regex IpAddressRegex();

    // TODO: [CONTRIBUTOR] Add check for homograph attacks (lookalike Unicode characters)
    // TODO: [CONTRIBUTOR] Add check for typosquatting (e.g., gooogle.com, microsof.com)
}
