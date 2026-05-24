using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsAndReceiptsAndDiscountForPrivateLastBillNo
{
    public DateTime RecDate { get; set; }

    public string Pno { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Billno { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string? Remarks { get; set; }

    public string Remarks2 { get; set; } = null!;

    public int Seed { get; set; }
}
