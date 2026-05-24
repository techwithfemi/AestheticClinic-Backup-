using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class MessageSpecial
{
    public long Sno { get; set; }

    public DateTime SendDate { get; set; }

    public DateTime? SendTime { get; set; }

    public string? Message { get; set; }

    public string CatName { get; set; } = null!;

    public string? Remarks { get; set; }
}
