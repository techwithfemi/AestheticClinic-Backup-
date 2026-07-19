namespace AestheticEMR.Core.Services.Accounting.Models;

public class AccountingReportYearLookup
{
    public string PeriodYr { get; set; } = string.Empty;
}

public class AccountingReportPeriodLookup
{
    public string Period { get; set; } = string.Empty;
    public string? PeriodVal { get; set; }
    public bool IsClose { get; set; }
    public DateTime PrdClose { get; set; }
}

public class AccountingLedgerLookup
{
    public string LedgerCode { get; set; } = string.Empty;
    public string Ledger { get; set; } = string.Empty;
}

public class AccountingAccountLookup
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}