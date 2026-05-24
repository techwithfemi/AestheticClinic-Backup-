using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAdmissionForDoctor
{
    public DateTime AdmDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? IsDischarged { get; set; }

    public string PNo { get; set; } = null!;
}
