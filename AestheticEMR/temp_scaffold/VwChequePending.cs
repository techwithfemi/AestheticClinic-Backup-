using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwChequePending
{
    public long Sno { get; set; }

    public string AcctName { get; set; } = null!;

    public string AcctIdsource { get; set; } = null!;

    public string? AcctIddest { get; set; }

    public double Amount { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ChequeDate { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? Remarks { get; set; }

    public bool? Returned { get; set; }

    public bool? Confirmed { get; set; }

    public DateTime TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public string CatHead { get; set; } = null!;

    public string? SubCat { get; set; }

    public string DrCr { get; set; } = null!;

    public string? BankCode { get; set; }

    public string? EntryBy { get; set; }

    public string? DeptId { get; set; }

    public string? Period { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? Status { get; set; }
}
