using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhMedChartForNurse
{
    public DateTime MDate { get; set; }

    public DateTime MTime { get; set; }

    public long Idno { get; set; }

    public string Drgname { get; set; } = null!;

    public long NumTaken { get; set; }

    public string? Nurse { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }

    public long Sno { get; set; }
}
