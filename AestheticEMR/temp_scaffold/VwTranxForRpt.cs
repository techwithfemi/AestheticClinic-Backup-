using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxForRpt
{
    public long Sno { get; set; }

    public DateTime TranDate { get; set; }

    public DateTime? ValueDate { get; set; }

    public string AcctId { get; set; } = null!;

    public string AcctName { get; set; } = null!;

    public string? ChequeNo { get; set; }

    public string? Remarks { get; set; }

    public double Debit { get; set; }

    public double Credit { get; set; }

    public double? Balance { get; set; }
}
