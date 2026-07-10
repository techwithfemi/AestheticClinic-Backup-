namespace AestheticEMR.Server.ViewModels.Accounting;

public class BalanceSheetHeaderVM
{
    public string? ItemName { get; set; }
    public string? RptType { get; set; }
    public string Period { get; set; } = string.Empty;
    public string CoyID { get; set; } = string.Empty;
}
