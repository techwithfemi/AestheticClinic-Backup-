using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHapptLatest
{
    public long Id { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ApptTime { get; set; }

    public string? ApptTime2 { get; set; }

    public string Patient { get; set; } = null!;

    public string? GivenBy { get; set; }

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }

    public bool? AttendedTo { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? ConId { get; set; }

    public string? PhoneNo { get; set; }

    public bool? AttendedToByRec { get; set; }

    public string? ClinicType { get; set; }
}
