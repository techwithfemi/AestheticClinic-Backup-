using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwVerDateForBillAccum
{
    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string? ConId { get; set; }

    public DateTime CDate { get; set; }
}
