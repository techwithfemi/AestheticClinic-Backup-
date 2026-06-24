using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AppDefault
{
    public string ID { get; set; } = null!;

    public string IDVal { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool Editable { get; set; }
}
