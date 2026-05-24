using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAdmissionWaitList
{
    public DateTime? Date { get; set; }

    public DateTime? Time { get; set; }

    public string? RoomNo { get; set; }

    public string? Doctor { get; set; }

    public byte NumOfPat { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? ReferTo { get; set; }

    public bool? AttendedTo { get; set; }

    public string? RefReason { get; set; }

    public string Patient { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? Sex { get; set; }

    public DateTime? Dob { get; set; }

    public int? Age { get; set; }
}
