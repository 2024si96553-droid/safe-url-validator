using SafeUrl.Models;
using SafeUrl.Rules;

namespace SafeUrl.Checker;

/// <summary>
/// Default implementation of URL safety checker.
/// Runs all registered rules against a URL and aggregates results.
/// </summary>
public class SafetyChecker : ISafetyChecker
{
    private readonly List<ISafetyRule> _rules = new();

    /// <inheritdoc />
    public IReadOnlyList<ISafetyRule> Rules => _rules.AsReadOnly();

    /// <summary>
    /// Creates a new safety checker with default rules.
    /// </summary>
    /// <param name="useDefaultRules">Whether to register default rules.</param>
    public SafetyChecker(bool useDefaultRules = true)
    {
        if (useDefaultRules)
        {
            RegisterDefaultRules();
        }
    }

    /// <summary>
    /// Registers the default set of safety rules.
    /// </summary>
    private void RegisterDefaultRules()
    {
        _rules.Add(new HttpsRule());
        _rules.Add(new SuspiciousTldRule());
        _rules.Add(new UrlLengthRule());
        _rules.Add(new SuspiciousDomainRule());

        // TODO: [CONTRIBUTOR] Add ThreatListRule that checks against public threat lists
        // TODO: [CONTRIBUTOR] Add RedirectChainRule that checks for suspicious redirect patterns
    }

    /// <inheritdoc />
    public void AddRule(ISafetyRule rule)
    {
        if (rule == null)
        {
            throw new ArgumentNullException(nameof(rule));
        }

        if (_rules.Any(r => r.RuleId == rule.RuleId))
        {
            throw new ArgumentException($"Rule with ID '{rule.RuleId}' already exists", nameof(rule));
        }

        _rules.Add(rule);
    }

    /// <inheritdoc />
    public bool RemoveRule(string ruleId)
    {
        var rule = _rules.FirstOrDefault(r => r.RuleId == ruleId);
        if (rule != null)
        {
            return _rules.Remove(rule);
        }
        return false;
    }

    /// <inheritdoc />
    public SafetyResult Check(string url)
    {
        return CheckAsync(url).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task<SafetyResult> CheckAsync(string url, CancellationToken cancellationToken = default)
    {
        var result = new SafetyResult
        {
            Url = url ?? string.Empty,
            CheckedAt = DateTime.UtcNow
        };

        if (string.IsNullOrWhiteSpace(url))
        {
            result.IsSuccess = false;
            result.ErrorMessage = "URL cannot be null or empty";
            result.Status = SafetyStatus.Unknown;
            return result;
        }

        try
        {
            foreach (var rule in _rules)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var issues = await rule.CheckAsync(url, cancellationToken);
                result.Issues.AddRange(issues);
            }

            result.IsSuccess = true;
            result.Score = CalculateScore(result.Issues);
            result.Status = DetermineStatus(result.Issues, result.Score);
        }
        catch (OperationCanceledException)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Check was cancelled";
            result.Status = SafetyStatus.Unknown;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"Error during safety check: {ex.Message}";
            result.Status = SafetyStatus.Unknown;
        }

        return result;
    }

    /// <summary>
    /// Calculates a safety score based on found issues.
    /// </summary>
    private static int CalculateScore(List<SafetyIssue> issues)
    {
        if (issues.Count == 0)
        {
            return 100;
        }

        var score = 100;

        foreach (var issue in issues)
        {
            score -= issue.Severity switch
            {
                IssueSeverity.Info => 0,
                IssueSeverity.Low => 5,
                IssueSeverity.Medium => 15,
                IssueSeverity.High => 30,
                IssueSeverity.Critical => 50,
                _ => 0
            };
        }

        return Math.Max(0, score);
    }

    /// <summary>
    /// Determines the overall safety status based on issues and score.
    /// </summary>
    private static SafetyStatus DetermineStatus(List<SafetyIssue> issues, int score)
    {
        if (issues.Any(i => i.Severity == IssueSeverity.Critical))
        {
            return SafetyStatus.Malicious;
        }

        if (issues.Any(i => i.Severity == IssueSeverity.High))
        {
            return SafetyStatus.Unsafe;
        }

        if (score < 50)
        {
            return SafetyStatus.Unsafe;
        }

        if (score < 80)
        {
            return SafetyStatus.Suspicious;
        }

        return SafetyStatus.Safe;
    }

    // TODO: [CONTRIBUTOR] Add method to check URL against threat list APIs
    // Example: Google Safe Browsing, PhishTank, URLhaus

    // TODO: [CONTRIBUTOR] Add caching for recently checked URLs

    // TODO: [CONTRIBUTOR] Add method to check entire redirect chain
}
