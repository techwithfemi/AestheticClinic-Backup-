using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class BillAccum
{
    public long SNo { get; set; }

    public DateTime dtDate { get; set; } = DateTime.Now;

    public string consultID { get; set; } = null!; // consultID of the attendee

    public string drgName { get; set; } = null!; // billitem name

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal? subTotal { get; set; }

    public string pNO { get; set; } = null!;

    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; } = true;

    public string? conID { get; set; }

    public bool? suppres { get; set; } = false;

    public string? Capitated { get; set; } = "NO";

    public bool? isBilled { get; set; } = true;

    public string? Usage { get; set; }

    public string? Category { get; set; } = string.Empty;

    public decimal? SubTotalSys { get; set; }

    public string CoyName { get; set; } = null!; // retainid of the attendee

    public string BillTo { get; set; } = null!; //same as CoyName

    public string? revType { get; set; }

    public string? DRGCode { get; set; }

    public bool? isRct { get; set; } = false;

    public bool isPost { get; set; } = false;

    public string? BillBy { get; set; } //empid of the biller

    public string? treatedBy { get; set; }

    public string? Dept { get; set; }

    public bool? isOLD { get; set; } = false;

    public string? RevClinic { get; set; } // clinictype of the attendee

    public int? AppVersion { get; set; } = 0;

    public bool? Reversed { get; set; } = false;

    public string? Remarks { get; set; }

    public string? TranID { get; set; }

    public long? ReversedPair { get; set; } = null;

    public string? AppName { get; set; }

    public string? ClientName { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }
}
