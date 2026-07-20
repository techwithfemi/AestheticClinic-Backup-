namespace AestheticEMR.Server.Services.Reporting;

public interface ILegacyCrystalReportProxyService
{
    Task<LegacyCrystalReportPayload> GetGeneralLedgerReportAsync(string coyID, string period, string ledgerCode, string accountNo, string? ledgerDisplayText, string? accountDisplayText, string? companyName, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetBalanceSheetReportAsync(string coyID, string period, string year, string rptBy, bool isClose, string? companyName, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetProfitAndLossReportAsync(string coyID, string period, string year, string rptBy, bool isClose, string? companyName, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetProfitAndLossDetailsReportAsync(string coyID, string period, string year, string rptBy, string groupID, bool isClose, string? companyName, CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetStaffRosterReportAsync(string coyID, string month, string year, string deptID, bool isClose, string? companyName, CancellationToken cancellationToken);
}

public sealed class LegacyCrystalReportPayload
{
    public required byte[] Content { get; init; }
    public required string ContentType { get; init; }
    public string? FileName { get; init; }
}
