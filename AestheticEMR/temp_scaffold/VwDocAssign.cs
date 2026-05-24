using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocAssign
{
    public DateTime Date { get; set; }

    public string RoomNo { get; set; } = null!;

    public string? Location { get; set; }

    public long Sno { get; set; }

    public string EmpId { get; set; } = null!;

    public string? DocName { get; set; }

    public bool? IsOff { get; set; }
}
