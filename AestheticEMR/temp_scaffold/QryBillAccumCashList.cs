using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumCashList
{
    public int Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string PatNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? CoyName { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string Fullname { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public string ClinicType { get; set; } = null!;
}
