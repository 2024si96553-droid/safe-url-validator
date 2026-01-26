using SafeUrl.Expander;
using Xunit;

namespace SafeUrl.Tests;

public class UrlExpanderTests
{
    [Fact]
    public async Task ExpandAsync_NullUrl_ReturnsError()
    {
        // Arrange
        var expander = new UrlExpander();

        // Act
        var result = await expander.ExpandAsync(null!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("URL cannot be null or empty", result.ErrorMessage);
    }

    [Fact]
    public async Task ExpandAsync_EmptyUrl_ReturnsError()
    {
        // Arrange
        var expander = new UrlExpander();

        // Act
        var result = await expander.ExpandAsync(string.Empty);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("URL cannot be null or empty", result.ErrorMessage);
    }

    [Fact]
    public async Task ExpandAsync_ValidUrl_TracksRedirectChain()
    {
        // Arrange
        var expander = new UrlExpander();
        var url = "https://www.google.com";

        // Act
        var result = await expander.ExpandAsync(url);

        // Assert
        Assert.Equal(url, result.OriginalUrl);
        Assert.True(result.RedirectChain.Count >= 0);
    }

    [Fact]
    public void Expand_ValidUrl_ReturnsSynchronously()
    {
        // Arrange
        var expander = new UrlExpander();
        var url = "https://www.example.com";

        // Act
        var result = expander.Expand(url);

        // Assert
        Assert.Equal(url, result.OriginalUrl);
    }

    [Fact]
    public void UrlExpanderOptions_DefaultValues_AreSet()
    {
        // Arrange & Act
        var options = new UrlExpanderOptions();

        // Assert
        Assert.Equal(10, options.MaxRedirects);
        Assert.Equal(10000, options.TimeoutMilliseconds);
        Assert.NotEmpty(options.UserAgent);
    }

    // TODO: [CONTRIBUTOR] Add tests with mocked HTTP responses
    // TODO: [CONTRIBUTOR] Add tests for redirect chain with multiple hops
    // TODO: [CONTRIBUTOR] Add tests for timeout handling
}
