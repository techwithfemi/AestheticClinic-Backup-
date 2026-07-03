using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Accounting;

public class ExpenseAccountLookupVM
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}

public class ExpenseEntryVM
{
    public long? SNo { get; set; }

    [Required(ErrorMessage = "Tran Date is required")]
    public DateTime TranDate { get; set; }

    [Required(ErrorMessage = "Expense account is required")]
    [StringLength(50)]
    public string AccountDebit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Paying account is required")]
    [StringLength(50)]
    public string AccountCredit { get; set; } = string.Empty;

    public string? DebitAccountName { get; set; }
    public string? CreditAccountName { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(250)]
    public string Description { get; set; } = string.Empty;

    public bool IsPost { get; set; }
    public bool PostDirectly { get; set; }
    public bool IsClose { get; set; }
    public string? UserName { get; set; }
    public string? TranId { get; set; }
    public string? Remarks { get; set; }
}

public class ExpenseListItemVM
{
    public long SNo { get; set; }
    public DateTime TranDate { get; set; }
    public string AccountDebit { get; set; } = string.Empty;
    public string AccountCredit { get; set; } = string.Empty;
    public string DebitAccountName { get; set; } = string.Empty;
    public string CreditAccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public bool IsPost { get; set; }
    public bool IsClose { get; set; }
    public string? UserName { get; set; }
    public string? TranId { get; set; }
    public string? Remarks { get; set; }
}

public class ExpenseListQueryVM
{
    [StringLength(100)]
    public string? Search { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    [StringLength(20)]
    public string ViewMode { get; set; } = "all";

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 200)]
    public int PageSize { get; set; } = 10;
}

public class PagedExpenseResultVM
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ExpenseListItemVM> Items { get; set; } = new();
}
