using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseApprvFirst
{
    public long ExpId { get; set; }

    public DateTime Date { get; set; }

    public string ExpName { get; set; } = null!;

    public string VouchNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public bool? IsApprv { get; set; }

    public string? Receivedby { get; set; }

    public string? Remarks { get; set; }

    public string ExpCat { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? SubTotal { get; set; }

    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? PersNo { get; set; }

    public DateTime? ExpDate { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }

    public string? FirstApprvBy { get; set; }

    public string CatType { get; set; } = null!;

    public string? CatCode { get; set; }

    public long? ExpIdSno { get; set; }
}
