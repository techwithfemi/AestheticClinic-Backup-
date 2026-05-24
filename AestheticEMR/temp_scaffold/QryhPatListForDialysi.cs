using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPatListForDialysi
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ClientCat { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public string? Remarks { get; set; }

    public string ClinicType { get; set; } = null!;

    public int? Age { get; set; }

    public string Company { get; set; } = null!;

    public int RecId { get; set; }

    public DateTime? Htime { get; set; }

    public bool? Suppres { get; set; }

    public string? Title { get; set; }

    public string RetainName { get; set; } = null!;

    public string? Sex { get; set; }

    public string? OldPno { get; set; }
}
