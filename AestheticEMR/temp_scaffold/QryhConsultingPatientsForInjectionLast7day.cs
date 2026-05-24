using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForInjectionLast7day
{
    public long Id { get; set; }

    public DateTime InjDate { get; set; }

    public string PSurName { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string ConsultId { get; set; } = null!;

    public long? ConId { get; set; }
}
