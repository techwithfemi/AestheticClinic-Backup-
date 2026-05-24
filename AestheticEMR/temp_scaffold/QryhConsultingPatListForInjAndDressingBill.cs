using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForInjAndDressingBill
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Treatedby { get; set; } = null!;

    public int? Age { get; set; }

    public string? InjPrescription { get; set; }
}
