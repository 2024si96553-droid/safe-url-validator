using SafeUrl.Models;

namespace SafeUrl.Rules;

/// <summary>
/// Rule that checks if a URL uses HTTPS.
/// </summary>
public class HttpsRule : ISafetyRule
{
    public string RuleId => "HTTPS_CHECK";
    public string Name => "HTTPS Check";
    public string Description => "Checks if the URL uses secure HTTPS protocol";

    public IEnumerable<SafetyIssue> Check(string url)
    {
        var issues = new List<SafetyIssue>();

        if (string.IsNullOrWhiteSpace(url))
        {
            return issues;
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            if (uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
            {
                issues.Add(new SafetyIssue
                {
                    RuleId = RuleId,
                    Description = "URL uses insecure HTTP instead of HTTPS",
                    Severity = IssueSeverity.Medium,
                    AffectedUrl = url
                });
            }
        }

        return issues;
    }

    public Task<IEnumerable<SafetyIssue>> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Check(url));
    }
}
