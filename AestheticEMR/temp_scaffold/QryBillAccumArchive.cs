using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumArchive
{
    public int Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Service { get; set; } = null!;

    public double UnitPrice { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string? PatNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Billtype { get; set; }

    public bool? AttendedTo { get; set; }

    public string? CatRemarks { get; set; }

    public string? RetainId { get; set; }

    public string PCatId { get; set; } = null!;

    public string? CoyName { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? ConId { get; set; }
}
