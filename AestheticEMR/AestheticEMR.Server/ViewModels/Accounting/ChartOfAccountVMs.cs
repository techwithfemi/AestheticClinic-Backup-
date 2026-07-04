using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Accounting;

public class ChartOfAccountListQueryVM
{
    [StringLength(100)]
    public string? Search { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 200)]
    public int PageSize { get; set; } = 10;

    [StringLength(50)]
    public string? SortBy { get; set; }

    [StringLength(4)]
    public string? SortDirection { get; set; }
}

public class ChartOfAccountListItemVM
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

public class ChartOfAccountEntryVM
{
    public long? SNo { get; set; }

    [StringLength(50)]
    public string AccountNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter Account Name")]
    [StringLength(200)]
    public string AccountName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter Account Group Name")]
    [StringLength(50)]
    public string GroupID { get; set; } = string.Empty;

    public string? GroupName { get; set; }

    [StringLength(500)]
    public string? AccountDesc { get; set; }

    public decimal AccountOpAmt { get; set; }
    public decimal AccountClAmt { get; set; }
}

public class ChartOfAccountGroupLookupVM
{
    public string GroupID { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
}

public class ChartOfAccountDefaultsVM
{
    public string AutoAccountNo { get; set; } = "YES";
    public string ReceiveExtData { get; set; } = "NO";
    public string ReceiveArData { get; set; } = "NO";
    public string ReceiveApData { get; set; } = "NO";
    public string ReceiveExpenseData { get; set; } = "NO";
    public string ReceivePayrollData { get; set; } = "NO";
}

public class PagedChartOfAccountResultVM
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ChartOfAccountListItemVM> Items { get; set; } = new();
}
