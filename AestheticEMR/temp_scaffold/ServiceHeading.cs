using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ServiceHeading
{
    public string Category { get; set; } = null!;

    public string? Heading { get; set; }

    public long? AcctId { get; set; }

    public long? AcctIdval { get; set; }
}
