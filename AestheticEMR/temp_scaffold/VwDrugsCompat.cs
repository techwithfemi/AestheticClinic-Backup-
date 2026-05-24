using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugsCompat
{
    public long Sno { get; set; }

    public string DrugCode { get; set; } = null!;

    public string DrugCodeIncompat { get; set; } = null!;
}
