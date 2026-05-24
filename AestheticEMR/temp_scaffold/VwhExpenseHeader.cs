using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseHeader
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public string VouchNo { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string? Remarks { get; set; }

    public decimal Amount { get; set; }

    public string? PersNo { get; set; }

    public bool? IsDone { get; set; }

    public bool? Suppres { get; set; }

    public string CatType { get; set; } = null!;
}
