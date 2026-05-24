using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScreeningAmount
{
    public long Sno { get; set; }

    public string CoyCode { get; set; } = null!;

    public double Amount { get; set; }

    public string ScreenName { get; set; } = null!;

    public byte[] Remarks { get; set; } = null!;
}
