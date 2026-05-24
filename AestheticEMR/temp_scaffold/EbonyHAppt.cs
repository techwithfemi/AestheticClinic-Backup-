using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EbonyHAppt
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ClinicType { get; set; }

    public string? ConId { get; set; }
}
