using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpense
{
    public long ExpId { get; set; }

    public DateTime Date { get; set; }

    public string? Remarks { get; set; }

    public string ExpName { get; set; } = null!;

    public string VouchNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public bool? IsApprv { get; set; }

    public string? Receivedby { get; set; }

    public string ExpCat { get; set; } = null!;

    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? PersNo { get; set; }

    public DateTime? ExpDate { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }
}
