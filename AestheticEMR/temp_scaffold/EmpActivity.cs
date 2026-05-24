using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpActivity
{
    public int ActId { get; set; }

    public DateTime? ActDate { get; set; }

    public string? Nature { get; set; }

    public decimal? Income { get; set; }

    public decimal? Expense { get; set; }

    public string? ApprovedBy { get; set; }

    public string? Remarks { get; set; }

    public string? EmpId { get; set; }

    public string? ActCatId { get; set; }
}
