using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForDispOffline
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? AttendedToByPharm { get; set; }

    public string? Prescription { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? Treatedby { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? CoyName { get; set; }

    public DateTime? CTime { get; set; }

    public string? BillRemarks { get; set; }

    public string? Referal { get; set; }
}
