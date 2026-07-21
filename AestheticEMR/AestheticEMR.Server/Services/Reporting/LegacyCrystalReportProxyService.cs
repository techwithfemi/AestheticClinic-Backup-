using System.Net.Http.Headers;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Reporting;

/// <summary>
/// Proxy service that forwards legacy Crystal report requests to CrystalReportWebAPI.
/// 
/// Database Connection Mapping:
/// - Accounting Reports (GL, BalanceSheet, P&L, etc.) → use "AccountingConnection" (Accounting DB)
/// - Staff Roster & Employee Reports → use "smartHRConnection" (SmartHR/Hospital DB)
/// - EMR/Clinical Reports (Invoice, ClosedJob, etc.) → use "DefaultConnection" (Hospital/EMR DB)
/// 
/// All report methods MUST explicitly specify which database connection to use.
/// No fallback or default values allowed - explicit connection selection only.
/// </summary>
public class LegacyCrystalReportProxyService(
    IHttpClientFactory httpClientFactory,
    IOptions<AppSettings> appSettings,
    IConfiguration configuration,
    ILogger<LegacyCrystalReportProxyService> logger) : ILegacyCrystalReportProxyService
{
    public async Task<LegacyCrystalReportPayload> GetGeneralLedgerReportAsync(string coyID, string period, string ledgerCode, string accountNo, string? ledgerDisplayText, string? accountDisplayText, string? companyName, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["ledgerCode"] = ledgerCode,
            ["accountNo"] = accountNo,
            ["ledgerDisplayText"] = ledgerDisplayText,
            ["accountDisplayText"] = accountDisplayText,
            ["companyName"] = companyName
        };

        var response = await SendGetAsync("Accounting/GeneralLedger", query, "AccountingConnection", cancellationToken);
        return await BuildPayloadAsync(response, $"general-ledger-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetBalanceSheetReportAsync(string coyID, string period, string year, string rptBy, bool isClose, string? companyName, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["isClose"] = isClose.ToString().ToLowerInvariant(),
            ["companyName"] = companyName
        };

        var response = await SendGetAsync("Accounting/BalanceSheet", query, "AccountingConnection", cancellationToken);
        return await BuildPayloadAsync(response, $"balance-sheet-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetProfitAndLossReportAsync(string coyID, string period, string year, string rptBy, bool isClose, string? companyName, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["isClose"] = isClose.ToString().ToLowerInvariant(),
            ["companyName"] = companyName
        };

        var response = await SendGetAsync("Accounting/ProfitAndLoss", query, "AccountingConnection", cancellationToken);
        return await BuildPayloadAsync(response, $"profit-and-loss-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetProfitAndLossDetailsReportAsync(string coyID, string period, string year, string rptBy, string groupID, bool isClose, string? companyName, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["period"] = period,
            ["year"] = year,
            ["rptBy"] = rptBy,
            ["groupID"] = groupID,
            ["isClose"] = isClose.ToString().ToLowerInvariant(),
            ["companyName"] = companyName
        };

        var response = await SendGetAsync("Accounting/ProfitAndLossDetails", query, "AccountingConnection", cancellationToken);
        return await BuildPayloadAsync(response, $"profit-and-loss-details-{period}.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken)
    {
        // Financial/VarianceAnalysisReport uses EMR database (Hospital), not Accounting
        var response = await SendGetAsync("Financial/VarianceAnalysisReport", null, "DefaultConnection", cancellationToken);
        return await BuildPayloadAsync(response, "variance-analysis-report.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken)
    {
        // Demonstration/ComparativeIncomeStatement uses EMR database (Hospital), not Accounting
        var response = await SendGetAsync("Demonstration/ComparativeIncomeStatement", null, "DefaultConnection", cancellationToken);
        return await BuildPayloadAsync(response, "comparative-income-statement.pdf", cancellationToken);
    }

    public async Task<LegacyCrystalReportPayload> GetStaffRosterReportAsync(string coyID, string month, string year, string deptID, bool isClose, string? companyName, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>
        {
            ["coyID"] = coyID,
            ["month"] = month,
            ["year"] = year,
            ["deptID"] = deptID,
            ["isClose"] = isClose.ToString().ToLowerInvariant(),
            ["companyName"] = companyName
        };

        // Staff Roster uses SmartHR database, not Accounting
        var response = await SendGetAsync("StaffRoster/Roster", query, "smartHRConnection", cancellationToken);
        return await BuildPayloadAsync(response, $"staff-roster-{deptID}-{month}.pdf", cancellationToken);
    }

    private async Task<HttpResponseMessage> SendGetAsync(string reportPath, IDictionary<string, string?>? query, string connectionStringKey, CancellationToken cancellationToken)
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

        if (string.IsNullOrWhiteSpace(connectionStringKey))
        {
            throw new InvalidOperationException("Connection string key must be specified.");
        }

        var connectionString = configuration.GetConnectionString(connectionStringKey);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionStringKey}' is missing.");
        }

        request.Headers.Add("X-Db-Connection", connectionString);

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

            var shortDetails = string.IsNullOrWhiteSpace(details)
                ? "No details returned."
                : details.Length > 500 ? details.Substring(0, 500) : details;

            throw new InvalidOperationException($"Legacy report service returned status {(int)response.StatusCode}. Details: {shortDetails}");
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
