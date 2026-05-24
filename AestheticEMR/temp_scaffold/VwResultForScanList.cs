using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwResultForScanList
{
    public string Fullname { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public long? ConId { get; set; }

    public string? Class { get; set; }
}
