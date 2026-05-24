using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAncObsteHist
{
    public long Sno { get; set; }

    public string AncregNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public DateTime? DtDelv { get; set; }

    public string? GestAge { get; set; }

    public string? DelvMode { get; set; }

    public string? WtAtBirth { get; set; }

    public string? Sex { get; set; }

    public string? CompAtBirth { get; set; }

    public string? Alive { get; set; }

    public string? Hosp { get; set; }
}
