using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhScanObstetricsList
{
    public long ConId { get; set; }

    public DateTime Invdate { get; set; }

    public string? LabNo { get; set; }

    public string Fullname { get; set; } = null!;
}
