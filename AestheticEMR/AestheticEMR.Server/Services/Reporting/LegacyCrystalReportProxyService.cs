using System.Net.Http.Headers;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Reporting;

public class LegacyCrystalReportProxyService(
    IHttpClientFactory httpClientFactory,
    IOptions<AppSettings> appSettings,
    ILogger<LegacyCrystalReportProxyService> logger) : ILegacyCrystalReportProxyService
{
    public async Task<LegacyCrystalReportPayload> GetGeneralLedgerReportAsync(string coyID, string period, string ledgerCode, string accountNo, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["ledgerCode"] = ledgerCode,
            ["accountNo"] = accountNo
        };

        var response = await SendGetAsync("Accounting/GeneralLedger", query, cancellationToken);
        return await BuildPayloadAsync(response, $"general-ledger-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetBalanceSheetReportAsync(string coyID, string period, string year, string rptBy, bool isClose, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["isClose"] = isClose.ToString().ToLowerInvariant()
        };

        var response = await SendGetAsync("Accounting/BalanceSheet", query, cancellationToken);
        return await BuildPayloadAsync(response, $"balance-sheet-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetProfitAndLossReportAsync(string coyID, string period, string year, string rptBy, bool isClose, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["isClose"] = isClose.ToString().ToLowerInvariant()
        };

        var response = await SendGetAsync("Accounting/ProfitAndLoss", query, cancellationToken);
        return await BuildPayloadAsync(response, $"profit-and-loss-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetProfitAndLossDetailsReportAsync(string coyID, string period, string year, string rptBy, string groupID, bool isClose, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["groupID"] = groupID,
            ["isClose"] = isClose.ToString().ToLowerInvariant()
        };

        var response = await SendGetAsync("Accounting/ProfitAndLossDetails", query, cancellationToken);
        return await BuildPayloadAsync(response, $"profit-and-loss-details-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken)
    {
        var response = await SendGetAsync("Financial/VarianceAnalysisReport", null, cancellationToken);
        return await BuildPayloadAsync(response, "variance-analysis-report.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken)
    {
        var response = await SendGetAsync("Demonstration/ComparativeIncomeStatement", null, cancellationToken);
        return await BuildPayloadAsync(response, "comparative-income-statement.pdf", cancellationToken);
    }

    private async Task<HttpResponseMessage> SendGetAsync(string reportPath, IDictionary<string, string?>? query, CancellationToken cancellationToken)
    {
        var cfg = appSettings.Value.LegacyReportService;
        if (cfg is null || string.IsNullOrWhiteSpace(cfg.BaseUrl))
        {
            throw new InvalidOperationException("Legacy report service configuration is missing. Set LegacyReportService:BaseUrl in appsettings.");
        }

        var baseUrl = cfg.BaseUrl.TrimEnd('/');
        var routePrefix = string.IsNullOrWhiteSpace(cfg.AccountingRoutePrefix) ? "api/Reports" : cfg.AccountingRoutePrefix.Trim('/');
        var requestUrl = BuildUrl(baseUrl, routePrefix, reportPath, query);

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

    private static string BuildUrl(string baseUrl, string routePrefix, string reportPath, IDictionary<string, string?>? query)
    {
        var url = $"{baseUrl}/{routePrefix}/{reportPath}";
        if (query is null || query.Count == 0)
        {
            return url;
        }

        var queryString = string.Join("&", query
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value!)}"));

        return string.IsNullOrWhiteSpace(queryString) ? url : $"{url}?{queryString}";
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
