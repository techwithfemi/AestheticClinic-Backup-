using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForInjection
{
    public long Id { get; set; }

    public DateTime InjDate { get; set; }

    public string Pno { get; set; } = null!;

    public string PSurName { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string InjName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public int? NumOfTimes { get; set; }

    public int? NumTaken { get; set; }

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Dosage { get; set; }

    public long? ConId { get; set; }

    public int? RowAge { get; set; }

    public string Company { get; set; } = null!;

    public DateTime? InjTime { get; set; }
}
