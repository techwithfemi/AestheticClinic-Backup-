using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCogsandSale
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Price { get; set; }

    public decimal? CostAmount { get; set; }

    public decimal? SalesAmount { get; set; }

    public bool? IsPost { get; set; }

    public string? AcctId { get; set; }

    public bool? Suppres { get; set; }

    public string Remarks { get; set; } = null!;

    public decimal? Margin { get; set; }

    public string? TranId { get; set; }

    public long? ReversedPair { get; set; }

    public string? Remarks2 { get; set; }

    public bool? Reversed { get; set; }
}
