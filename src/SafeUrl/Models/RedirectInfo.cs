namespace SafeUrl.Models;

/// <summary>
/// Represents a single redirect in the URL chain.
/// </summary>
public class RedirectInfo
{
    /// <summary>
    /// The URL at this step in the redirect chain.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// The HTTP status code returned (e.g., 301, 302, 307).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The position in the redirect chain (0 = original URL).
    /// </summary>
    public int Step { get; set; }
}
