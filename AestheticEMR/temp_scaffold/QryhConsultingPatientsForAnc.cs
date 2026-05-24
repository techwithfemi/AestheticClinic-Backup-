using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForAnc
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedTo { get; set; }

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public bool? Suppres { get; set; }

    public string ClinicType { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Sex { get; set; }
}
