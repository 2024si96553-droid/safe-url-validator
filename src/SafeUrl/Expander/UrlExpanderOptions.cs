namespace SafeUrl.Expander;

/// <summary>
/// Configuration options for the URL expander.
/// </summary>
public class UrlExpanderOptions
{
    /// <summary>
    /// Maximum number of redirects to follow. Default is 10.
    /// </summary>
    public int MaxRedirects { get; set; } = 10;

    /// <summary>
    /// Request timeout in milliseconds. Default is 10000 (10 seconds).
    /// </summary>
    public int TimeoutMilliseconds { get; set; } = 10000;

    /// <summary>
    /// User agent string to use for requests.
    /// </summary>
    public string UserAgent { get; set; } = "SafeUrl/1.0 (URL Safety Checker)";

    // TODO: [CONTRIBUTOR] Add option to follow meta refresh redirects
    // public bool FollowMetaRefresh { get; set; } = false;

    // TODO: [CONTRIBUTOR] Add option for proxy configuration
    // public string? ProxyUrl { get; set; }

    // TODO: [CONTRIBUTOR] Add option to cache expanded URLs
    // public bool EnableCaching { get; set; } = false;
    // public int CacheExpirationMinutes { get; set; } = 60;
}
