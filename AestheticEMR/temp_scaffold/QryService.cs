using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryService
{
    public long? AcctId { get; set; }

    public string? Code { get; set; }

    public string? CodeItem { get; set; }

    public string? Heading { get; set; }

    public string? Category { get; set; }

    public string? Remarks { get; set; }

    public bool? IsHeading { get; set; }

    public long? AcctIdval { get; set; }

    public string? CodeAndItem { get; set; }
}
