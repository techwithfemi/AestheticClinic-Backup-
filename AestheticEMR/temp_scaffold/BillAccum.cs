using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillAccum
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string PNo { get; set; } = null!;

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? Capitated { get; set; }

    public bool? IsBilled { get; set; }

    public string? Usage { get; set; }

    public string? Category { get; set; }

    public decimal? SubTotalSys { get; set; }

    public string BillTo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? RevType { get; set; }

    public string? Drgcode { get; set; }

    public bool? IsRct { get; set; }

    public bool IsPost { get; set; }

    public string? BillBy { get; set; }

    public string? TreatedBy { get; set; }

    public string? Dept { get; set; }

    public bool? IsOld { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? RevClinic { get; set; }

    public int? AppVersion { get; set; }

    public bool? Reversed { get; set; }

    public string? Remarks { get; set; }

    public string? TranId { get; set; }

    public long? ReversedPair { get; set; }
}
