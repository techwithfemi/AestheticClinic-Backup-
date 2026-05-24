using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class CrosstabTest
{
    public int? Sno { get; set; }

    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? PCatId { get; set; }

    public string RevType { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string Service { get; set; } = null!;

    public double SubTotal { get; set; }

    public string? ClientCatId { get; set; }
}
