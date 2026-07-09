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

/// <summary>
/// Flat row from <c>vwTranx</c>. Mirrors the SQL projection in
/// <see cref="IJournalEntryService.GetPagedLinesAsync"/> so the front-end
/// grid is a 1:1 view of the underlying view, with derived Dr/Cr amounts
/// and a running serial number.
/// </summary>
public class JournalListLine
{
    public long SN { get; set; }
    public DateTime TranDate { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNo { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
    public string TranNo { get; set; } = string.Empty;
    public string? TranCat { get; set; }
    public string? BillNo { get; set; }
    public string? CostCenter { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Period { get; set; }
    public string? UserName { get; set; }
    public long SNo { get; set; }
    public string? Remarks { get; set; }
    public string CoyID { get; set; } = string.Empty;
    public bool IsClose { get; set; }
}

/// <summary>
/// Query for the flat <c>vwTranx</c> list. When <see cref="Search"/> is
/// null/empty the service falls back to <see cref="TranDate"/> (which the
/// front-end defaults to "today"); when search is supplied the date filter
/// is ignored so the user can find a TranNo across all dates.
/// </summary>
public class JournalListLineQuery
{
    public string? Search { get; set; }
    public DateTime? TranDate { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedJournalLinesResult
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<JournalListLine> Items { get; set; } = new();
}