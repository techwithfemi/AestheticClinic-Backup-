using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockTable
{
    public long Sno { get; set; }

    public string TblName { get; set; } = null!;

    public string ColName { get; set; } = null!;

    public string? Remarks { get; set; }
}
