using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpenseDetailsApprvFirst
{
    public long ExpId { get; set; }

    public DateTime? ExpDate { get; set; }

    public string VouchNo { get; set; } = null!;

    public string ExpName { get; set; } = null!;

    public string ExpCat { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? SubTotal { get; set; }

    public long? AcctId { get; set; }

    public bool? IsApprv { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Remarks { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }

    public bool? IsDone { get; set; }

    public long? ExpIdSno { get; set; }
}
