namespace IpShieldMiddleware.Models;

public class IpFilterOptions
{
    /// <summary>
    /// A list of IPs that are allowed to access the application.
    /// </summary>
    public List<string> WhitelistedIps { get; set; } = new();

    /// <summary>
    /// A list of IPs that are denied from accessing the application.
    /// </summary>
    public List<string> BlacklistedIps { get; set; } = new();

    /// <summary>
    /// Optional custom message for denied IPs. Use `{IP}` as a placeholder.
    /// </summary>
    public string? CustomErrorMessage { get; set; }

    /// <summary>
    /// Optional custom log message template for denied IPs. Use `{IP}` as a placeholder.
    /// </summary>
    public string? CustomLogMessage { get; set; }
}
