using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseApprvFinal
{
    public long ExpId { get; set; }

    public long Sno { get; set; }

    public string VouchNo { get; set; } = null!;

    public long ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public DateTime? ExpDate { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? SubTotal { get; set; }

    public string? ReceivedBy { get; set; }

    public string? ApprvBy { get; set; }

    public string? AcctId { get; set; }

    public string? RefNo { get; set; }

    public bool? IsPost { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? IsApprv { get; set; }

    public bool? IsPaid { get; set; }

    public decimal AmountApprved { get; set; }

    public string CatCode { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string CatType { get; set; } = null!;

    public bool? Suppres { get; set; }

    public string Remarks { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public long? ExpIdSno { get; set; }
}
