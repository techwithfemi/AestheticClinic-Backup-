using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxaction2
{
    public long Sno { get; set; }

    public DateTime AttndDate { get; set; }

    public DateTime BillDate { get; set; }

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal Bill { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal RunningTotal { get; set; }

    public string? Remarks { get; set; }

    public string Comapany { get; set; } = null!;

    public int Seed { get; set; }
}
