using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForLabAttendance
{
    public int Id { get; set; }

    public DateTime CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedToByDoc { get; set; }

    public string Prescription { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string Treatedby { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? CoyName { get; set; }

    public DateTime? Htime { get; set; }

    public bool? Suppres { get; set; }

    public string? Referal { get; set; }

    public string? Ref { get; set; }

    public string RetainName { get; set; } = null!;

    public string Services { get; set; } = null!;
}
