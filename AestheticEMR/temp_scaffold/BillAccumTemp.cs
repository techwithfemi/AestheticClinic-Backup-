using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillAccumTemp
{
    public int Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string PNo { get; set; } = null!;

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? Capitated { get; set; }

    public bool? IsBilled { get; set; }

    public string? Usage { get; set; }

    public string? Category { get; set; }

    public double? SubTotalSys { get; set; }

    public string? BillTo { get; set; }

    public string? CoyName { get; set; }

    public string? RevType { get; set; }
}
