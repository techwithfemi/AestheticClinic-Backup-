using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpenseDetail
{
    public long ExpId { get; set; }

    public string VouchNo { get; set; } = null!;

    public string ExpName { get; set; } = null!;

    public string ExpCat { get; set; } = null!;

    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    public long? AcctId { get; set; }

    public bool? IsApprv { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ExpDate { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }
}
