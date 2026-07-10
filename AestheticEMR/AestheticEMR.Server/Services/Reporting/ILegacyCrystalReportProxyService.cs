namespace AestheticEMR.Server.Services.Reporting;

public interface ILegacyCrystalReportProxyService
{
    Task<LegacyCrystalReportPayload> GetFinancialVarianceAnalysisReportAsync(CancellationToken cancellationToken);
    Task<LegacyCrystalReportPayload> GetComparativeIncomeStatementReportAsync(CancellationToken cancellationToken);
}

public sealed class LegacyCrystalReportPayload
{
    public required byte[] Content { get; init; }
    public required string ContentType { get; init; }
    public string? FileName { get; init; }
}
