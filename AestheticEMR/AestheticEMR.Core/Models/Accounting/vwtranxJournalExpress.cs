using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwtranxJournalExpress
{
    public long SNo { get; set; }

    public DateTime TranDate { get; set; }

    public string CenterName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string DivName { get; set; } = null!;

    public string? CoyID { get; set; }
}
