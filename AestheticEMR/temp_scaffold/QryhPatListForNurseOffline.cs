using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPatListForNurseOffline
{
    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? ClientCat { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public string? Remarks { get; set; }

    public string ClinicType { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? CoyName { get; set; }

    public int RecId { get; set; }

    public DateTime? Htime { get; set; }

    public bool? Suppres { get; set; }
}
