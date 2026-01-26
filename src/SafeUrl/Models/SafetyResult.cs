namespace SafeUrl.Models;

/// <summary>
/// Overall safety status of a URL.
/// </summary>
public enum SafetyStatus
{
    /// <summary>
    /// URL appears safe.
    /// </summary>
    Safe,

    /// <summary>
    /// URL has some suspicious characteristics.
    /// </summary>
    Suspicious,

    /// <summary>
    /// URL is potentially dangerous.
    /// </summary>
    Unsafe,

    /// <summary>
    /// URL is a known threat.
    /// </summary>
    Malicious,

    /// <summary>
    /// Could not determine safety status.
    /// </summary>
    Unknown
}

/// <summary>
/// Represents the complete safety analysis result for a URL.
/// </summary>
public class SafetyResult
{
    /// <summary>
    /// The URL that was checked.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// The overall safety status.
    /// </summary>
    public SafetyStatus Status { get; set; }

    /// <summary>
    /// List of all safety issues found.
    /// </summary>
    public List<SafetyIssue> Issues { get; set; } = new();

    /// <summary>
    /// Safety score from 0 (dangerous) to 100 (safe).
    /// </summary>
    public int Score { get; set; } = 100;

    /// <summary>
    /// Whether the check was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if the check failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Timestamp when the check was performed.
    /// </summary>
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Returns true if the URL is considered safe (no high/critical issues).
    /// </summary>
    public bool IsSafe => Status == SafetyStatus.Safe;
}
