using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranx
{
    public DateTime TrDate { get; set; }

    public string AcctId { get; set; } = null!;

    public string CatHead { get; set; } = null!;

    public string? SubCat { get; set; }

    public string DrCr { get; set; } = null!;

    public double Amount { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? ChequeNo { get; set; }
}
