using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Billingdetails100919
{
    public string BillNo { get; set; } = null!;

    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string? BillType { get; set; }

    public string? ConId { get; set; }

    public string? Capitated { get; set; }

    public string? Dosage { get; set; }

    public string? Category { get; set; }

    public string? BillTo { get; set; }

    public string? CoyName { get; set; }

    public string? BillHead { get; set; }

    public string? RevType { get; set; }

    public string? Drgcode { get; set; }

    public bool IsPost { get; set; }

    public bool? IsRct { get; set; }

    public string? BillBy { get; set; }

    public string? TreatedBy { get; set; }

    public string? Dept { get; set; }

    public bool? IsOld { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
