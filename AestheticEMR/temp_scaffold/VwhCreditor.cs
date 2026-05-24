using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhCreditor
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public string BillNo { get; set; } = null!;

    public string? Company { get; set; }

    public double Amount { get; set; }

    public string? Remarks { get; set; }

    public string? Description { get; set; }

    public string? ClientNo { get; set; }

    public string? AcctId { get; set; }

    public double? Balance { get; set; }
}
