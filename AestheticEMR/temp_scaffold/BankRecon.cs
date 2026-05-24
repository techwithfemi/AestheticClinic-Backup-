using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BankRecon
{
    public long Sno { get; set; }

    public DateTime? DateVal { get; set; }

    public string? ItemId { get; set; }

    public string? BankCode { get; set; }

    public string? AcctId { get; set; }

    public string? Remarks { get; set; }

    public double? Amount { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }
}
