using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxCreditor
{
    public long Sno { get; set; }

    public DateTime TranDate { get; set; }

    public string AcctId { get; set; } = null!;

    public string? AcctName { get; set; }

    public string? AcctGp { get; set; }

    public string CatHead { get; set; } = null!;

    public string? SubCat { get; set; }

    public DateTime ValueDate { get; set; }

    public string? DrCr { get; set; }

    public string ChequeNo { get; set; } = null!;

    public string? Remarks { get; set; }

    public double? OpenBal { get; set; }

    public double Debit { get; set; }

    public double? Credit { get; set; }

    public double? Balance { get; set; }
}
