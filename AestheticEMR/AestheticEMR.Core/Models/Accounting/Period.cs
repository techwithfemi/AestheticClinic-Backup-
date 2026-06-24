using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Period
{
    public string Period1 { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string? Remarks { get; set; }

    public long SNo { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
