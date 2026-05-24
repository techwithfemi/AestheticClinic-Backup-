using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPatientClinic
{
    public long Sno { get; set; }

    public string Pno { get; set; } = null!;

    public string Clinic { get; set; } = null!;

    public DateTime RegDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public bool? Active { get; set; }

    public string? Remarks { get; set; }
}
