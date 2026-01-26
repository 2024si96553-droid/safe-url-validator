namespace SafeUrl.Domain;

/// <summary>
/// Interface for retrieving domain information.
///
/// CONTRIBUTORS WANTED: This entire module needs implementation!
///
/// Features to implement:
/// - WHOIS lookup (domain age, registrar, expiration)
/// - DNS records (A, AAAA, MX, TXT, NS)
/// - SSL certificate information
/// - Geolocation of server
/// </summary>
public interface IDomainInfo
{
    /// <summary>
    /// Gets information about a domain.
    /// </summary>
    /// <param name="domain">The domain name to look up.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Information about the domain.</returns>
    Task<DomainDetails> GetInfoAsync(string domain, CancellationToken cancellationToken = default);
}

/// <summary>
/// Detailed information about a domain.
///
/// TODO: [CONTRIBUTOR] Add more domain fields as needed
/// </summary>
public class DomainDetails
{
    /// <summary>
    /// The domain name.
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// When the domain was registered.
    /// </summary>
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// When the domain expires.
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Domain registrar name.
    /// </summary>
    public string? Registrar { get; set; }

    /// <summary>
    /// Age of the domain in days.
    /// </summary>
    public int? AgeDays => CreatedDate.HasValue
        ? (int)(DateTime.UtcNow - CreatedDate.Value).TotalDays
        : null;

    /// <summary>
    /// DNS A records (IP addresses).
    /// </summary>
    public List<string> ARecords { get; set; } = new();

    /// <summary>
    /// DNS MX records (mail servers).
    /// </summary>
    public List<string> MxRecords { get; set; } = new();

    /// <summary>
    /// Country where the server is located.
    /// </summary>
    public string? ServerCountry { get; set; }

    /// <summary>
    /// Whether SSL certificate is valid.
    /// </summary>
    public bool? SslValid { get; set; }

    /// <summary>
    /// SSL certificate expiration date.
    /// </summary>
    public DateTime? SslExpiration { get; set; }

    /// <summary>
    /// Whether the lookup was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if lookup failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    // TODO: [CONTRIBUTOR] Add ASN information
    // public string? Asn { get; set; }
    // public string? AsnOrganization { get; set; }

    // TODO: [CONTRIBUTOR] Add nameserver information
    // public List<string> Nameservers { get; set; }
}
