using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocMinNumOfPatOld
{
    public DateTime? Date { get; set; }

    public string? RoomNo { get; set; }

    public string? DocName { get; set; }

    public int? NumOfPat { get; set; }

    public bool? IsOff { get; set; }

    public string EmpId { get; set; } = null!;
}
