using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class BillAccum
{
    public long SNo { get; set; }

    public DateTime dtDate { get; set; }

    public string consultID { get; set; } = null!;

    public string drgName { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal? subTotal { get; set; }

    public string pNO { get; set; } = null!;

    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    public string? conID { get; set; }

    public bool? suppres { get; set; }

    public string? Capitated { get; set; }

    public bool? isBilled { get; set; }

    public string? Usage { get; set; }

    public string? Category { get; set; }

    public decimal? SubTotalSys { get; set; }

    public string BillTo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? revType { get; set; }

    public string? DRGCode { get; set; }

    public bool? isRct { get; set; }

    public bool isPost { get; set; }

    public string? BillBy { get; set; }

    public string? treatedBy { get; set; }

    public string? Dept { get; set; }

    public bool? isOLD { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? RevClinic { get; set; }

    public int? AppVersion { get; set; }

    public bool? Reversed { get; set; }

    public string? Remarks { get; set; }

    public string? TranID { get; set; }

    public long? ReversedPair { get; set; }
}
