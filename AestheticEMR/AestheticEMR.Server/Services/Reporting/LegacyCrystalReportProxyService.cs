using System.Net.Http.Headers;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Reporting;

public class LegacyCrystalReportProxyService(
    IHttpClientFactory httpClientFactory,
    IOptions<AppSettings> appSettings,
    ILogger<LegacyCrystalReportProxyService> logger) : ILegacyCrystalReportProxyService
{
    public async Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken)
    {
        var response = await SendGetAsync("Financial/VarianceAnalysisReport", cancellationToken);
        return await BuildPayloadAsync(response, "variance-analysis-report.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken)
    {
        var response = await SendGetAsync("Demonstration/ComparativeIncomeStatement", cancellationToken);
        return await BuildPayloadAsync(response, "comparative-income-statement.pdf", cancellationToken);
    }

    private async Task<HttpResponseMessage> SendGetAsync(string reportPath, CancellationToken cancellationToken)
    {
        var cfg = appSettings.Value.LegacyReportService;
        if (cfg is null || string.IsNullOrWhiteSpace(cfg.BaseUrl))
        {
            throw new InvalidOperationException("Legacy report service configuration is missing. Set LegacyReportService:BaseUrl in appsettings.");
        }

        var baseUrl = cfg.BaseUrl.TrimEnd('/');
        var routePrefix = string.IsNullOrWhiteSpace(cfg.AccountingRoutePrefix) ? "api/Reports" : cfg.AccountingRoutePrefix.Trim('/');
        var requestUrl = $"{baseUrl}/{routePrefix}/{reportPath}";

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        if (!string.IsNullOrWhiteSpace(cfg.ApiKey))
        {
            request.Headers.Add("X-Api-Key", cfg.ApiKey);
        }

        var client = httpClientFactory.CreateClient(nameof(LegacyCrystalReportProxyService));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning("Legacy report service call failed. Url: {Url}, Status: {StatusCode}, Details: {Details}", requestUrl, (int)response.StatusCode, details);
            response.Dispose();
            throw new InvalidOperationException($"Legacy report service returned status {(int)response.StatusCode}.");
        }

        return response;
    }

    private static async Task<LegacyCrystalReportPayload> BuildPayloadAsync(HttpResponseMessage response, string fallbackFileName, CancellationToken cancellationToken)
    {
        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var ms = new MemoryStream();
        await contentStream.CopyToAsync(ms, cancellationToken);

        var contentType = response.Content.Headers.ContentType?.MediaType;
        var fileName = ReadFileName(response.Content.Headers.ContentDisposition) ?? fallbackFileName;

        response.Dispose();

        return new LegacyCrystalReportPayload
        {
            Content = ms.ToArray(),
            ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/pdf" : contentType,
            FileName = fileName
        };
    }

    private static string? ReadFileName(ContentDispositionHeaderValue? contentDisposition)
    {
        if (contentDisposition is null)
        {
            return null;
        }

        return contentDisposition.FileNameStar?.Trim('"')
               ?? contentDisposition.FileName?.Trim('"');
    }
}
