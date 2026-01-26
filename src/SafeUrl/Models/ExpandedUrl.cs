namespace SafeUrl.Models;

/// <summary>
/// Represents the result of expanding a shortened URL.
/// </summary>
public class ExpandedUrl
{
    /// <summary>
    /// The original shortened URL that was provided.
    /// </summary>
    public required string OriginalUrl { get; set; }

    /// <summary>
    /// The final destination URL after following all redirects.
    /// </summary>
    public required string FinalUrl { get; set; }

    /// <summary>
    /// The complete chain of redirects from original to final URL.
    /// </summary>
    public List<RedirectInfo> RedirectChain { get; set; } = new();

    /// <summary>
    /// The total number of redirects encountered.
    /// </summary>
    public int RedirectCount => RedirectChain.Count;

    /// <summary>
    /// Whether the expansion was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if expansion failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Time taken to expand the URL in milliseconds.
    /// </summary>
    public long ElapsedMilliseconds { get; set; }
}
