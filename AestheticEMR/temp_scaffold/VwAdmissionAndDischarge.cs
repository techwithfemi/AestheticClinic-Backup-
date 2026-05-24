using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAdmissionAndDischarge
{
    public long Id { get; set; }

    public DateTime AdmDate { get; set; }

    public DateTime DischDate { get; set; }

    public int? NumDays { get; set; }

    public string ConsultId { get; set; } = null!;
}
