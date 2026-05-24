using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HreferalArchive
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string ReferTo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string RefReason { get; set; } = null!;

    public DateTime? RefDate { get; set; }

    public DateTime? RefTime { get; set; }

    public bool? AttendedTo { get; set; }

    public string? RefAddress { get; set; }
}
