namespace AestheticEMR.Core.Services.Accounting.Models;

public class ChartOfAccountListQuery
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

public class ChartOfAccountListItem
{
    public long SNo { get; set; }
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string GroupID { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string? AccountDesc { get; set; }
    public decimal AccountOpAmt { get; set; }
    public decimal AccountClAmt { get; set; }
}

public class ChartOfAccountEntry
{
    public long? SNo { get; set; }
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string GroupID { get; set; } = string.Empty;
    public string? GroupName { get; set; }
    public string? AccountDesc { get; set; }
    public decimal AccountOpAmt { get; set; }
    public decimal AccountClAmt { get; set; }
}

public class ChartOfAccountGroupLookup
{
    public string GroupID { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
}

public class ChartOfAccountDefaults
{
    public string AutoAccountNo { get; set; } = "YES";
    public string ReceiveExtData { get; set; } = "NO";
    public string ReceiveArData { get; set; } = "NO";
    public string ReceiveApData { get; set; } = "NO";
    public string ReceiveExpenseData { get; set; } = "NO";
    public string ReceivePayrollData { get; set; } = "NO";
}

public class PagedChartOfAccountResult
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ChartOfAccountListItem> Items { get; set; } = new();
}
