namespace SafeUrl.Metadata;

/// <summary>
/// Interface for extracting metadata from URLs.
///
/// CONTRIBUTORS WANTED: This entire module needs implementation!
///
/// Features to implement:
/// - Page title extraction
/// - Favicon URL extraction
/// - Meta description
/// - Open Graph tags
/// - Server headers analysis
/// </summary>
public interface IMetadataParser
{
    /// <summary>
    /// Extracts metadata from a URL.
    /// </summary>
    /// <param name="url">The URL to extract metadata from.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Metadata about the URL.</returns>
    Task<UrlMetadata> ParseAsync(string url, CancellationToken cancellationToken = default);
}

/// <summary>
/// Metadata extracted from a URL.
///
/// TODO: [CONTRIBUTOR] Add more metadata fields as needed
/// </summary>
public class UrlMetadata
{
    /// <summary>
    /// The URL that was parsed.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Page title from the HTML.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// URL of the favicon.
    /// </summary>
    public string? FaviconUrl { get; set; }

    /// <summary>
    /// Meta description from the page.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Content-Type header from the server.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Server header value.
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// Whether the metadata extraction was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if extraction failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    // TODO: [CONTRIBUTOR] Add Open Graph properties
    // public string? OgTitle { get; set; }
    // public string? OgImage { get; set; }
    // public string? OgDescription { get; set; }
}
