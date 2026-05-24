using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhAppoint
{
    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string Patient { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ClinicType { get; set; }

    public string? GivenBy { get; set; }

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? Company { get; set; }

    public bool? AttendedTo { get; set; }
}
