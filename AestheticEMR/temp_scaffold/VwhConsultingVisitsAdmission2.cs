using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingVisitsAdmission2
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? Referal { get; set; }

    public string RetainName { get; set; } = null!;

    public string? Remarks { get; set; }
}
