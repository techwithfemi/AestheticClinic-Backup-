using Microsoft.AspNetCore.Http;
using System;

namespace AestheticEMR.Core.Infrastructure;

public sealed class AuditRequestContext(IHttpContextAccessor httpContextAccessor)
{
    public string? GetIpAddress()
        => FirstHeaderValue("X-Forwarded-For")
           ?? httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? GetUserAgent()
        => httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

    public string? GetRequestPath()
        => httpContextAccessor.HttpContext?.Request.Path.Value;

    public string? GetDeviceName()
        => FirstHeaderValue("X-Device-Name")
           ?? FirstHeaderValue("X-Client-Device")
           ?? httpContextAccessor.HttpContext?.Request.Headers.Host.ToString();

    public string? GetCity()
        => FirstHeaderValue("X-City")
           ?? FirstHeaderValue("X-Client-City");

    public string? GetCountry()
        => FirstHeaderValue("X-Country")
           ?? FirstHeaderValue("X-Client-Country");

    public string? GetCoordinates()
        => FirstHeaderValue("X-Coordinates")
           ?? FirstHeaderValue("X-Client-Coordinates");

    private string? FirstHeaderValue(string headerName)
    {
        var raw = httpContextAccessor.HttpContext?.Request.Headers[headerName].ToString();
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var first = raw.Split(',')[0].Trim();
        return string.IsNullOrWhiteSpace(first) ? null : first;
    }
}
