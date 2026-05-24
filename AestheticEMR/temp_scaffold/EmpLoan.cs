using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpLoan
{
    public DateTime? LoanDate { get; set; }

    public string? EmpId { get; set; }

    public string RefNo { get; set; } = null!;

    public decimal? Amount { get; set; }

    public int? PayDuration { get; set; }

    public string? ApprovedBy { get; set; }

    public string? LoanCatId { get; set; }
}
