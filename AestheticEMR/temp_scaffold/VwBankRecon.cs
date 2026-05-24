using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBankRecon
{
    public long Sno { get; set; }

    public DateTime? Date { get; set; }

    public string? Item { get; set; }

    public string? AcctId { get; set; }

    public double? Amount { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }
}
