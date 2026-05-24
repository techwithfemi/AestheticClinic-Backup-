using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForLabCopy
{
    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Investigate { get; set; }

    public DateTime InvDate { get; set; }

    public bool? AttendedTo { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Treatedby { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? CoyName { get; set; }
}
