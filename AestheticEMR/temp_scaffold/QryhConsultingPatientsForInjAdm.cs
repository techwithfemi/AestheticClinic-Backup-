using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForInjAdm
{
    public long Id { get; set; }

    public DateTime InjDate { get; set; }

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedTo { get; set; }

    public string InjName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public int? NumOfTimes { get; set; }

    public int? NumTaken { get; set; }

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Dosage { get; set; }

    public string Fullname { get; set; } = null!;
}
