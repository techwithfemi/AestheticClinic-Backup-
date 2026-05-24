using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryHconsultingPatientsForEye
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }
}
