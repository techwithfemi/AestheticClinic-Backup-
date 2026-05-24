using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillAccumSumm
{
    public string ConsultId { get; set; } = null!;

    public decimal? SubTotal { get; set; }

    public string PNo { get; set; } = null!;
}
