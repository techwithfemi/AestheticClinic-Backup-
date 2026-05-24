using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QrybillAccumHome
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Service { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string PatNo { get; set; } = null!;

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string CatRemarks { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string? PCatId { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Capitated { get; set; }

    public string? Category { get; set; }

    public double? Debt { get; set; }

    public string? Usage { get; set; }

    public string BillTo { get; set; } = null!;

    public string? Coyname { get; set; }

    public bool? Reversed { get; set; }
}
