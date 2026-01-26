namespace SafeUrl.Models;

/// <summary>
/// Severity level of a safety issue.
/// </summary>
public enum IssueSeverity
{
    /// <summary>
    /// Informational - not necessarily a problem.
    /// </summary>
    Info,

    /// <summary>
    /// Low risk - minor concern.
    /// </summary>
    Low,

    /// <summary>
    /// Medium risk - should be reviewed.
    /// </summary>
    Medium,

    /// <summary>
    /// High risk - likely malicious.
    /// </summary>
    High,

    /// <summary>
    /// Critical - known threat.
    /// </summary>
    Critical
}

/// <summary>
/// Represents a single safety concern found during URL analysis.
/// </summary>
public class SafetyIssue
{
    /// <summary>
    /// The rule or check that triggered this issue.
    /// </summary>
    public required string RuleId { get; set; }

    /// <summary>
    /// Human-readable description of the issue.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The severity level of this issue.
    /// </summary>
    public IssueSeverity Severity { get; set; }

    /// <summary>
    /// The specific URL in the chain that triggered this issue.
    /// </summary>
    public string? AffectedUrl { get; set; }
}
