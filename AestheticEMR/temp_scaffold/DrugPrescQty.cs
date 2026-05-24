using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugPrescQty
{
    public long Sno { get; set; }

    public string Description { get; set; } = null!;

    public decimal Qty { get; set; }

    public string Remarks { get; set; } = null!;
}
