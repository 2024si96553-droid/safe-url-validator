using SafeUrl;
using SafeUrl.Models;

Console.WriteLine("===========================================");
Console.WriteLine("   SafeUrl - URL Expander & Safety Checker");
Console.WriteLine("===========================================\n");

using var client = new SafeUrlClient();

// Test URLs to demonstrate the library
var testUrls = new[]
{
    "https://www.google.com",                    // Safe URL
    "http://example.com",                         // HTTP (not HTTPS)
    "https://suspicious-site.tk",                 // Suspicious TLD
    "http://192.168.1.1/login",                   // IP address URL
    "https://secure-login-verify-account.com",   // Suspicious domain pattern
    "https://bit.ly/3ABC123",                     // URL shortener (will try to expand)
};

foreach (var url in testUrls)
{
    Console.WriteLine($"Analyzing: {url}");
    Console.WriteLine(new string('-', 50));

    try
    {
        var result = await client.AnalyzeAsync(url);

        // Show expansion results
        Console.WriteLine($"  Original URL:  {result.ExpandedUrl.OriginalUrl}");
        Console.WriteLine($"  Final URL:     {result.ExpandedUrl.FinalUrl}");
        Console.WriteLine($"  Redirects:     {result.ExpandedUrl.RedirectCount}");
        Console.WriteLine($"  Expand Time:   {result.ExpandedUrl.ElapsedMilliseconds}ms");

        // Show safety results
        Console.WriteLine($"\n  Safety Status: {GetStatusEmoji(result.SafetyResult.Status)} {result.SafetyResult.Status}");
        Console.WriteLine($"  Safety Score:  {result.SafetyResult.Score}/100");

        if (result.SafetyResult.Issues.Count > 0)
        {
            Console.WriteLine($"\n  Issues Found ({result.SafetyResult.Issues.Count}):");
            foreach (var issue in result.SafetyResult.Issues)
            {
                Console.WriteLine($"    [{issue.Severity}] {issue.Description}");
            }
        }
        else
        {
            Console.WriteLine("\n  No issues found.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error: {ex.Message}");
    }

    Console.WriteLine("\n");
}

// Interactive mode
Console.WriteLine("===========================================");
Console.WriteLine("   Interactive Mode - Enter URLs to check");
Console.WriteLine("   (Type 'exit' to quit)");
Console.WriteLine("===========================================\n");

while (true)
{
    Console.Write("Enter URL: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    try
    {
        var result = await client.AnalyzeAsync(input);

        Console.WriteLine($"\n  Final URL:     {result.FinalUrl}");
        Console.WriteLine($"  Status:        {GetStatusEmoji(result.SafetyResult.Status)} {result.SafetyResult.Status}");
        Console.WriteLine($"  Score:         {result.SafetyResult.Score}/100");

        if (result.SafetyResult.Issues.Count > 0)
        {
            Console.WriteLine("  Issues:");
            foreach (var issue in result.SafetyResult.Issues)
            {
                Console.WriteLine($"    - {issue.Description}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error: {ex.Message}");
    }

    Console.WriteLine();
}

static string GetStatusEmoji(SafetyStatus status) => status switch
{
    SafetyStatus.Safe => "[OK]",
    SafetyStatus.Suspicious => "[WARN]",
    SafetyStatus.Unsafe => "[DANGER]",
    SafetyStatus.Malicious => "[BLOCKED]",
    _ => "[?]"
};
