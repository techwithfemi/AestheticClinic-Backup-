using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPastMedHistDrgRxn
{
    public string? Fullname { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? DrgRxn { get; set; }

    public string? PastMedHist { get; set; }

    public string? Ancinfo { get; set; }
}
