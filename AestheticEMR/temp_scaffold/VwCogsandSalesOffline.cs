using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCogsandSalesOffline
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Qty { get; set; }

    public double? Cost { get; set; }

    public double? Price { get; set; }

    public double? CostAmount { get; set; }

    public double? SalesAmount { get; set; }

    public bool? IsPost { get; set; }

    public string? AcctId { get; set; }

    public bool? Suppres { get; set; }

    public string Remarks { get; set; } = null!;

    public double? Margin { get; set; }
}
