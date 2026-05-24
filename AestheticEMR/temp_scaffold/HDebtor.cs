using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDebtor
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string ClientNo { get; set; } = null!;

    public double Amount { get; set; }

    public bool IsPaid { get; set; }

    public string? Remarks { get; set; }

    public string? InvNo { get; set; }
}
