using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForDisp
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedToByPharm { get; set; }

    public string? Prescription { get; set; }

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string? Treatedby { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? Coyname { get; set; }

    public DateTime? CTime { get; set; }

    public bool? Suppres { get; set; }

    public string? Referal { get; set; }

    public string? Ref { get; set; }

    public string RetainName { get; set; } = null!;

    public string? Services { get; set; }

    public string DocId { get; set; } = null!;

    public string? Diagnosis { get; set; }
}
