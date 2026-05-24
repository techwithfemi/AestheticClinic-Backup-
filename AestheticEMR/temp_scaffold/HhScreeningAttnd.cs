using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HhScreeningAttnd
{
    public long Sno { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Remarks { get; set; } = null!;
}
