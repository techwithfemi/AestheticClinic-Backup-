using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPorder
{
    public string Poid { get; set; } = null!;

    public int? Mth { get; set; }

    public int? Yr { get; set; }

    public DateTime? OrderDate { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? IsApprv { get; set; }
}
