using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConRoomAssign
{
    public long Sno { get; set; }

    public DateTime SchdDate { get; set; }

    public string ConRoomNo { get; set; } = null!;

    public string DocNo { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool? IsOff { get; set; }
}
