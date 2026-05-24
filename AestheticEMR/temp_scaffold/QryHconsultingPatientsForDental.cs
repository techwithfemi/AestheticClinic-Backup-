using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryHconsultingPatientsForDental
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;
}
