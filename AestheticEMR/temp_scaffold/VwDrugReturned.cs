using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugReturned
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public decimal Qty { get; set; }

    public decimal Cost { get; set; }

    public decimal? Amount { get; set; }

    public string? LocId { get; set; }

    public bool? Suppres { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }

    public string? Remarks { get; set; }

    public bool? Reversed { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? AcctId { get; set; }

    public string? BatchId { get; set; }

    public string? BatchNo { get; set; }
}
