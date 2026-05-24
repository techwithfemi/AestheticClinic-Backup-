using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhCapitation
{
    public string MthName { get; set; } = null!;

    public string Mth { get; set; } = null!;

    public string? Yr { get; set; }

    public string RetainId { get; set; } = null!;

    public string Company { get; set; } = null!;

    public double Amount { get; set; }

    public string? Remarks { get; set; }

    public long? Sno { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ClientCatId { get; set; }

    public string? ClientType { get; set; }

    public string CoyName { get; set; } = null!;
}
