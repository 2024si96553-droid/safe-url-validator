using SafeUrl.Models;

namespace SafeUrl.Expander;

/// <summary>
/// Interface for URL expansion services.
/// Implement this interface to create custom URL expanders.
/// </summary>
public interface IUrlExpander
{
    /// <summary>
    /// Expands a shortened URL to its final destination.
    /// </summary>
    /// <param name="url">The shortened URL to expand.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The expanded URL result containing the final URL and redirect chain.</returns>
    Task<ExpandedUrl> ExpandAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Expands a shortened URL synchronously.
    /// </summary>
    /// <param name="url">The shortened URL to expand.</param>
    /// <returns>The expanded URL result.</returns>
    ExpandedUrl Expand(string url);
}
