using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwVerifyDate
{
    public int Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime? CDate { get; set; }

    public DateTime RecDate { get; set; }

    public string DrgName { get; set; } = null!;
}
