using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Service
{
    public long? AcctId { get; set; }

    public string? ServCode { get; set; }

    public string? Service1 { get; set; }

    public string? Class { get; set; }

    public string? Category { get; set; }

    public string? Remarks { get; set; }

    public bool? IsHeading { get; set; }

    public long? AcctIdval { get; set; }
}
