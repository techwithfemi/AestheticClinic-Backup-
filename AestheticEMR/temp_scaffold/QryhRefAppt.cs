using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhRefAppt
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public string ReferTo { get; set; } = null!;

    public string RefReason { get; set; } = null!;

    public DateTime? RefDate { get; set; }

    public DateTime? RefTime { get; set; }

    public bool? AttendedTo { get; set; }
}
