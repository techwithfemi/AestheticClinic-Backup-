using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocWaitingListGrouped
{
    public DateTime Date { get; set; }

    public string? Doctor { get; set; }

    public string? DocName { get; set; }

    public int? NumOfPat { get; set; }

    public string? EmpId { get; set; }

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string RoomNo { get; set; } = null!;

    public int IsOff { get; set; }
}
