namespace AestheticEMR.Server.Services.Reporting;

public interface ILegacyCrystalReportProxyService
{
    Task<LegacyCrystalReportPayload> GetGeneralLedgerReportAsync(string coyID, string period, string ledgerCode, string accountNo, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetBalanceSheetReportAsync(string coyID, string period, string year, string rptBy, bool isClose, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetProfitAndLossReportAsync(string coyID, string period, string year, string rptBy, bool isClose, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetProfitAndLossDetailsReportAsync(string coyID, string period, string year, string rptBy, string groupID, bool isClose, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken);
}

public sealed class LegacyCrystalReportPayload
{
    public required byte[] Content { get; init; }
    public required string ContentType { get; init; }
    public string? FileName { get; init; }
}
