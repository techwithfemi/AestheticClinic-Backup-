using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugPriceSettingForPvtOld
{
    public long Sno { get; set; }

    public decimal MinValue { get; set; }

    public decimal MaxValue { get; set; }

    public decimal Pcent { get; set; }

    public string? Remarks { get; set; }
}
