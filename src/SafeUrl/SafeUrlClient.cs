using SafeUrl.Checker;
using SafeUrl.Expander;
using SafeUrl.Models;
using SafeUrl.Rules;

namespace SafeUrl;

/// <summary>
/// Main client for URL expansion and safety checking.
/// Provides a simple API to expand shortened URLs and check their safety.
/// </summary>
public class SafeUrlClient : IDisposable
{
    private readonly IUrlExpander _expander;
    private readonly ISafetyChecker _checker;
    private bool _disposed;

    /// <summary>
    /// Creates a new SafeUrl client with default settings.
    /// </summary>
    public SafeUrlClient()
    {
        _expander = new UrlExpander();
        _checker = new SafetyChecker();
    }

    /// <summary>
    /// Creates a new SafeUrl client with custom expander and checker.
    /// </summary>
    /// <param name="expander">Custom URL expander.</param>
    /// <param name="checker">Custom safety checker.</param>
    public SafeUrlClient(IUrlExpander expander, ISafetyChecker checker)
    {
        _expander = expander ?? throw new ArgumentNullException(nameof(expander));
        _checker = checker ?? throw new ArgumentNullException(nameof(checker));
        //Mock change
    }

    /// <summary>
    /// Gets the URL expander instance.
    /// </summary>
    public IUrlExpander Expander => _expander;

    /// <summary>
    /// Gets the safety checker instance.
    /// </summary>
    public ISafetyChecker Checker => _checker;

    /// <summary>
    /// Expands a shortened URL to its final destination.
    /// </summary>
    /// <param name="url">The shortened URL to expand.</param>
    /// <returns>The expanded URL result.</returns>
    public ExpandedUrl Expand(string url)
    {
        return _expander.Expand(url);
    }

    /// <summary>
    /// Asynchronously expands a shortened URL.
    /// </summary>
    /// <param name="url">The shortened URL to expand.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The expanded URL result.</returns>
    public Task<ExpandedUrl> ExpandAsync(string url, CancellationToken cancellationToken = default)
    {
        return _expander.ExpandAsync(url, cancellationToken);
    }

    /// <summary>
    /// Checks a URL for safety issues.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>The safety check result.</returns>
    public SafetyResult CheckSafety(string url)
    {
        return _checker.Check(url);
    }

    /// <summary>
    /// Asynchronously checks a URL for safety issues.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The safety check result.</returns>
    public Task<SafetyResult> CheckSafetyAsync(string url, CancellationToken cancellationToken = default)
    {
        return _checker.CheckAsync(url, cancellationToken);
    }

    /// <summary>
    /// Expands a URL and checks the final destination for safety.
    /// This is the main method for complete URL analysis.
    /// </summary>
    /// <param name="url">The URL to analyze.</param>
    /// <returns>Combined result with expansion and safety information.</returns>
    public UrlAnalysisResult Analyze(string url)
    {
        return AnalyzeAsync(url).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Asynchronously expands a URL and checks the final destination for safety.
    /// </summary>
    /// <param name="url">The URL to analyze.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Combined result with expansion and safety information.</returns>
    public async Task<UrlAnalysisResult> AnalyzeAsync(string url, CancellationToken cancellationToken = default)
    {
        var expandedResult = await _expander.ExpandAsync(url, cancellationToken);

        var urlToCheck = expandedResult.IsSuccess ? expandedResult.FinalUrl : url;
        var safetyResult = await _checker.CheckAsync(urlToCheck, cancellationToken);

        return new UrlAnalysisResult
        {
            ExpandedUrl = expandedResult,
            SafetyResult = safetyResult
        };
    }

    /// <summary>
    /// Adds a custom safety rule to the checker.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    public void AddRule(ISafetyRule rule)
    {
        _checker.AddRule(rule);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_expander is IDisposable disposableExpander)
                {
                    disposableExpander.Dispose();
                }
            }
            _disposed = true;
        }
    }
}

/// <summary>
/// Combined result of URL expansion and safety check.
/// </summary>
public class UrlAnalysisResult
{
    /// <summary>
    /// Result of URL expansion.
    /// </summary>
    public required ExpandedUrl ExpandedUrl { get; set; }

    /// <summary>
    /// Result of safety check on the final URL.
    /// </summary>
    public required SafetyResult SafetyResult { get; set; }

    /// <summary>
    /// Returns true if the URL is considered safe.
    /// </summary>
    public bool IsSafe => SafetyResult.IsSafe;

    /// <summary>
    /// The final destination URL.
    /// </summary>
    public string FinalUrl => ExpandedUrl.FinalUrl;
}
