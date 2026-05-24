using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPatientsImageUrl
{
    public long Sno { get; set; }

    public string Pno { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Remarks { get; set; }
}
