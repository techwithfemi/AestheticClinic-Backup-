namespace AestheticEMR.Server.ViewModels.Accounting;

public class AccountingReportPeriodVM
{
    public string Period { get; set; } = string.Empty;
    public string? PeriodVal { get; set; }
    public bool IsClose { get; set; }
    public DateTime PrdClose { get; set; }
}