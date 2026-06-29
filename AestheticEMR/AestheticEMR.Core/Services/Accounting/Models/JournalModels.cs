namespace AestheticEMR.Core.Services.Accounting.Models;

public class JournalLine
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
    public DateTime TranDate { get; set; }
}

public class JournalEntry
{
    public string TranNo { get; set; } = string.Empty;
    public DateTime TranDate { get; set; }
    public string CostCenterId { get; set; } = string.Empty;
    public string? CostCenterName { get; set; }
    public List<JournalLine> Lines { get; set; } = new();

    public decimal TotalDebit => Lines?.Sum(l => l.Debit) ?? 0m;
    public decimal TotalCredit => Lines?.Sum(l => l.Credit) ?? 0m;
    public decimal Balance => Math.Abs(TotalDebit - TotalCredit);
}

public class JournalAccountLookup
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}

public class JournalCostCenterLookup
{
    public string CenterId { get; set; } = string.Empty;
    public string CenterName { get; set; } = string.Empty;
}

public class JournalListItem
{
    public string TranNo { get; set; } = string.Empty;
    public DateTime TranDate { get; set; }
    public int LineCount { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public string CostCenterId { get; set; } = string.Empty;
    public string? CostCenterName { get; set; }
}

public class JournalListQuery
{
    public string? Search { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedJournalResult
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<JournalListItem> Items { get; set; } = new();
}