using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwRctForAcct
{
    public DateTime Date { get; set; }

    public string ConsultId { get; set; } = null!;

    public double? SubTotal { get; set; }

    public string? RevType { get; set; }

    public string AccountNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;
}
