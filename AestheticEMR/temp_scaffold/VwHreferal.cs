using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHreferal
{
    public DateTime? ApptDate { get; set; }

    public string? ApptTime { get; set; }

    public string Patient { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public string? EntryTime { get; set; }

    public string? ReferTo { get; set; }

    public string? GivenBy { get; set; }

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? AttendedToByRec { get; set; }

    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? ConId { get; set; }

    public string? PhoneNo { get; set; }

    public string? Remarks { get; set; }
}
