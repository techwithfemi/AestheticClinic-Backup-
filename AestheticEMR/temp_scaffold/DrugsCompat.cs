using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugsCompat
{
    public long Sno { get; set; }

    public string DrugCode { get; set; } = null!;

    public string DrugCodeIncompat { get; set; } = null!;
}
