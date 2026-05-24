using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDialysisDetail
{
    public long Sno { get; set; }

    public long? SnoId { get; set; }

    public string? ConsultId { get; set; }

    public DateTime? DialTime { get; set; }

    public string? Bp { get; set; }

    public string? Pulse { get; set; }

    public string? Bfr { get; set; }

    public string? Ufr { get; set; }

    public string? Np { get; set; }

    public string? Vp { get; set; }

    public string? Ivf { get; set; }

    public string? HepperHr { get; set; }

    public string? Remarks { get; set; }
}
