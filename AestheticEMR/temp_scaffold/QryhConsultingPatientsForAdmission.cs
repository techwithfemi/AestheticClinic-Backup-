using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForAdmission
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? IsDischarged { get; set; }
}
