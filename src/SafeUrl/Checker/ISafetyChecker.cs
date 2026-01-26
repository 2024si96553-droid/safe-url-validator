using SafeUrl.Models;
using SafeUrl.Rules;

namespace SafeUrl.Checker;

/// <summary>
/// Interface for URL safety checking services.
/// Implement this interface to create custom safety checkers.
/// </summary>
public interface ISafetyChecker
{
    /// <summary>
    /// Checks a URL for safety issues using all registered rules.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>Safety result containing all found issues.</returns>
    SafetyResult Check(string url);

    /// <summary>
    /// Asynchronously checks a URL for safety issues.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Safety result containing all found issues.</returns>
    Task<SafetyResult> CheckAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a custom safety rule to the checker.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    void AddRule(ISafetyRule rule);

    /// <summary>
    /// Removes a safety rule by its ID.
    /// </summary>
    /// <param name="ruleId">The ID of the rule to remove.</param>
    /// <returns>True if the rule was removed, false if not found.</returns>
    bool RemoveRule(string ruleId);

    /// <summary>
    /// Gets all registered rules.
    /// </summary>
    IReadOnlyList<ISafetyRule> Rules { get; }
}
