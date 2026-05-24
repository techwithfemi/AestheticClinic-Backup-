using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPix
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public DateTime DtTime { get; set; }

    public byte[]? Image { get; set; }

    public string Pno { get; set; } = null!;

    public string? ConsultId { get; set; }

    public string? Remarks { get; set; }
}
