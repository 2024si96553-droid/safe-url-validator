using System.Diagnostics;
using System.Net;
using SafeUrl.Models;

namespace SafeUrl.Expander;

/// <summary>
/// Default implementation of URL expander that follows HTTP redirects.
/// </summary>
public class UrlExpander : IUrlExpander
{
    private readonly HttpClient _httpClient;
    private readonly UrlExpanderOptions _options;

    /// <summary>
    /// Creates a new URL expander with default options.
    /// </summary>
    public UrlExpander() : this(new UrlExpanderOptions())
    {
    }

    /// <summary>
    /// Creates a new URL expander with custom options.
    /// </summary>
    /// <param name="options">Configuration options for the expander.</param>
    public UrlExpander(UrlExpanderOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            MaxAutomaticRedirections = _options.MaxRedirects
        };

        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromMilliseconds(_options.TimeoutMilliseconds)
        };

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
    }

    /// <summary>
    /// Creates a new URL expander with a custom HttpClient.
    /// </summary>
    /// <param name="httpClient">Custom HttpClient instance.</param>
    /// <param name="options">Configuration options.</param>
    public UrlExpander(HttpClient httpClient, UrlExpanderOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<ExpandedUrl> ExpandAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return new ExpandedUrl
            {
                OriginalUrl = url ?? string.Empty,
                FinalUrl = url ?? string.Empty,
                IsSuccess = false,
                ErrorMessage = "URL cannot be null or empty"
            };
        }

        var result = new ExpandedUrl
        {
            OriginalUrl = url,
            FinalUrl = url
        };

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var currentUrl = url;
            var redirectCount = 0;

            while (redirectCount < _options.MaxRedirects)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using var request = new HttpRequestMessage(HttpMethod.Head, currentUrl);
                using var response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                var statusCode = (int)response.StatusCode;

                result.RedirectChain.Add(new RedirectInfo
                {
                    Url = currentUrl,
                    StatusCode = statusCode,
                    Step = redirectCount
                });

                if (IsRedirectStatusCode(response.StatusCode))
                {
                    var location = response.Headers.Location;
                    if (location == null)
                    {
                        break;
                    }

                    currentUrl = location.IsAbsoluteUri
                        ? location.AbsoluteUri
                        : new Uri(new Uri(currentUrl), location).AbsoluteUri;

                    redirectCount++;
                }
                else
                {
                    break;
                }
            }

            result.FinalUrl = currentUrl;
            result.IsSuccess = true;
        }
        catch (TaskCanceledException)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Request timed out";
        }
        catch (HttpRequestException ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"HTTP error: {ex.Message}";
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"Unexpected error: {ex.Message}";
        }
        finally
        {
            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        return result;
    }

    /// <inheritdoc />
    public ExpandedUrl Expand(string url)
    {
        return ExpandAsync(url).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Checks if an HTTP status code indicates a redirect.
    /// </summary>
    private static bool IsRedirectStatusCode(HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.MovedPermanently ||    // 301
               statusCode == HttpStatusCode.Found ||                // 302
               statusCode == HttpStatusCode.SeeOther ||             // 303
               statusCode == HttpStatusCode.TemporaryRedirect ||    // 307
               statusCode == HttpStatusCode.PermanentRedirect;      // 308
    }

    // TODO: [CONTRIBUTOR] Add method to detect and handle URL shortener services
    // Examples: bit.ly, tinyurl.com, t.co, goo.gl, etc.
    // This could provide special handling or caching for known shorteners

    // TODO: [CONTRIBUTOR] Add method to extract and return URL parameters
    // This could help identify tracking parameters (utm_*, fbclid, etc.)

    // TODO: [CONTRIBUTOR] Add support for handling JavaScript-based redirects
    // Some URLs use meta refresh or JavaScript for redirection
}
