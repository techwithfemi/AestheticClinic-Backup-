using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HCreditor
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string? ClientNo { get; set; }

    public string? Pno { get; set; }

    public double? Balance { get; set; }

    public double Amount { get; set; }

    public bool? IsPaid { get; set; }

    public string? Remarks { get; set; }

    public string? Description { get; set; }
}
