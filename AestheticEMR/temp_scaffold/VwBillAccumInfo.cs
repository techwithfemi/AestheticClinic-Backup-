using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillAccumInfo
{
    public DateTime Date { get; set; }

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? PCatId { get; set; }

    public string BillNo { get; set; } = null!;

    public bool? IsBilled { get; set; }

    public string? ClientCatId { get; set; }

    public string? Ref { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public double SubTotal { get; set; }

    public string? Capitated { get; set; }
}
