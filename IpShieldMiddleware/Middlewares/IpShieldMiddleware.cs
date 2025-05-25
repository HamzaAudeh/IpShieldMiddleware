using IpShieldMiddleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace IpShieldMiddleware.Middlewares;

public class IpShieldMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IpFilterOptions _options;
    private readonly ILogger<IpShieldMiddleware> _logger;

    public IpShieldMiddleware(
        RequestDelegate next,
        IOptions<IpFilterOptions> options,
        ILogger<IpShieldMiddleware> logger)
    {
        _next = next;
        _options = options.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        IPAddress remoteIp = context.Connection.RemoteIpAddress;

        if (remoteIp is null)
        {
            string responseMessage = BuildResponseMessage("Access denied: Unable to determine client IP.", null);
            string logMessage = BuildLogMessage("Access denied: Unable to determine client IP.", null);
            _logger.LogWarning(responseMessage);
            await DenyRequestAsync(context, responseMessage);
            return;
        }

        string ipString = remoteIp.ToString();

        if (_options.WhitelistedIps.Any() && !_options.WhitelistedIps.Contains(remoteIp.ToString()))
        {
            string responseMessage = BuildResponseMessage("Access denied: '{IP}' is not whitelisted.", ipString);
            string logMessage = BuildLogMessage("Access denied: '{IP}' is not whitelisted.", ipString);
            _logger.LogWarning(logMessage);
            await DenyRequestAsync(context, responseMessage);
            return;
        }

        if (_options.BlacklistedIps.Contains(remoteIp.ToString()))
        {
            string responseMessage = BuildResponseMessage("Access denied: '{IP}' is blacklisted.", ipString);
            string logMessage = BuildLogMessage("Access denied: '{IP}' is blacklisted.", ipString);
            _logger.LogWarning(logMessage);
            await DenyRequestAsync(context, responseMessage);
            return;
        }

        await _next(context);
    }

    private string BuildResponseMessage(string defaultMessage, string? ip)
    {
        string? message = _options.CustomErrorMessage;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = defaultMessage;
        }

        if (!string.IsNullOrEmpty(ip))
        {
            message = message.Replace("{IP}", ip);
        }

        return message;
    }

    private string BuildLogMessage(string defaultMessage, string? ip)
    {
        string? message = _options.CustomLogMessage;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = defaultMessage;
        }

        if (!string.IsNullOrEmpty(ip))
        {
            message = message.Replace("{IP}", ip);
        }

        return message;
    }

    private static async Task DenyRequestAsync(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            statusCode = 403,
            message
        };

        var json = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(json);
    }
}
