using SafeUrl.Models;

namespace SafeUrl.Rules;

/// <summary>
/// Interface for safety rules that check URLs for potential threats.
/// Implement this interface to create custom safety rules.
/// </summary>
public interface ISafetyRule
{
    /// <summary>
    /// Unique identifier for this rule.
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Human-readable name of the rule.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Description of what this rule checks.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Checks a URL against this safety rule.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>List of safety issues found, or empty list if URL passes the rule.</returns>
    IEnumerable<SafetyIssue> Check(string url);

    /// <summary>
    /// Asynchronously checks a URL against this safety rule.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of safety issues found.</returns>
    Task<IEnumerable<SafetyIssue>> CheckAsync(string url, CancellationToken cancellationToken = default);
}
