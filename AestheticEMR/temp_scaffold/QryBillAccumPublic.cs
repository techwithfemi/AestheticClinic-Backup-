using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumPublic
{
    public int Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Service { get; set; } = null!;

    public double UnitPrice { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string PatNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string CatRemarks { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? ConId { get; set; }

    public string? Remarks { get; set; }
}
