namespace AestheticEMR.Core.Services.Audit;

public sealed class AdminAuditReportRow
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string UserAction { get; set; } = string.Empty;
    public string? OriginalAction { get; set; }
    public string? Remarks { get; set; }
    public string? Src { get; set; }
    public string? Employee { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string TranCode { get; set; } = string.Empty;
    public string? Module { get; set; }
}
