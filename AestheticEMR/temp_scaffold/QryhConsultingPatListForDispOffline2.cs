using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForDispOffline2
{
    public int Id { get; set; }

    public DateTime CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstName { get; set; } = null!;

    public bool? AttendedToByPharm { get; set; }

    public string Prescription { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string Treatedby { get; set; } = null!;

    public int Age { get; set; }

    public string Company { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public DateTime? Htime { get; set; }

    public bool? Suppres { get; set; }

    public string? Referal { get; set; }

    public string RetainName { get; set; } = null!;

    public string Services { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string? PPhoneNo { get; set; }
}
