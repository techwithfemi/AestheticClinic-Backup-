using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumUnion
{
    public int? Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string? Billtype { get; set; }
}
