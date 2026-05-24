using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugLocationOld
{
    public string LocId { get; set; } = null!;

    public string LocName { get; set; } = null!;

    public bool? AllowEntry { get; set; }

    public bool? CanIssue { get; set; }

    public bool? IsDummy { get; set; }

    public bool? IsBulkCost { get; set; }

    public bool? IsForValuation { get; set; }
}
