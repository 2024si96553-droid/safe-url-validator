using SafeUrl.Checker;
using SafeUrl.Models;
using Xunit;

namespace SafeUrl.Tests;

public class SafetyCheckerTests
{
    [Fact]
    public void Check_SafeHttpsUrl_ReturnsSafeStatus()
    {
        // Arrange
        var checker = new SafetyChecker();
        var url = "https://www.google.com";

        // Act
        var result = checker.Check(url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(SafetyStatus.Safe, result.Status);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public void Check_HttpUrl_ReturnsIssue()
    {
        // Arrange
        var checker = new SafetyChecker();
        var url = "http://www.example.com";

        // Act
        var result = checker.Check(url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.RuleId == "HTTPS_CHECK");
    }

    [Fact]
    public void Check_SuspiciousTld_ReturnsIssue()
    {
        // Arrange
        var checker = new SafetyChecker();
        var url = "https://suspicious-site.tk";

        // Act
        var result = checker.Check(url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.RuleId == "SUSPICIOUS_TLD");
    }

    [Fact]
    public void Check_IpAddressUrl_ReturnsHighSeverityIssue()
    {
        // Arrange
        var checker = new SafetyChecker();
        var url = "http://192.168.1.1/login";

        // Act
        var result = checker.Check(url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Issues, i =>
            i.RuleId == "SUSPICIOUS_DOMAIN" &&
            i.Severity == IssueSeverity.High);
    }

    [Fact]
    public void Check_NullUrl_ReturnsError()
    {
        // Arrange
        var checker = new SafetyChecker();

        // Act
        var result = checker.Check(null!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(SafetyStatus.Unknown, result.Status);
    }

    [Fact]
    public void Check_LongUrl_ReturnsLengthIssue()
    {
        // Arrange
        var checker = new SafetyChecker();
        var url = "https://example.com/" + new string('a', 150);

        // Act
        var result = checker.Check(url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.RuleId == "URL_LENGTH");
    }

    // TODO: [CONTRIBUTOR] Add more test cases for edge cases
    // TODO: [CONTRIBUTOR] Add integration tests with real URL shorteners
}
