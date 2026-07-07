namespace AestheticEMR.Core.Services.Accounting.Models;

public class ExpenseAccountLookup
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}

public class ExpenseEntry
{
    public long? SNo { get; set; }
    public DateTime TranDate { get; set; }
    public string AccountDebit { get; set; } = string.Empty;
    public string AccountCredit { get; set; } = string.Empty;
    public string? DebitAccountName { get; set; }
    public string? CreditAccountName { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPost { get; set; }
    public bool IsClose { get; set; }
    public string? UserName { get; set; }
    public string? TranId { get; set; }
    public string? Remarks { get; set; }
}

public class ExpenseBatchSaveRequest
{
    public string? TranId { get; set; }
    public List<ExpenseEntry> Entries { get; set; } = new();
}

public class ExpenseBatchSaveResult
{
    public List<ExpenseEntry> Entries { get; set; } = new();
}

public class ExpenseTranIdResult
{
    public string TranId { get; set; } = string.Empty;
}

public class ExpenseListItem
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
    public string Period { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public long SNo { get; set; }
    public string? Remarks { get; set; }
    public string? CoyID { get; set; }
    public bool IsClose { get; set; }
}

public class ExpenseListQuery
{
    public string? Search { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedExpenseResult
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ExpenseListItem> Items { get; set; } = new();
}
