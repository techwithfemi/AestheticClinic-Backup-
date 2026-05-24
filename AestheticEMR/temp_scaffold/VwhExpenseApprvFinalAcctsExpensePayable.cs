using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseApprvFinalAcctsExpensePayable
{
    public long Sno { get; set; }

    public string VouchNo { get; set; } = null!;

    public long ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? SubTotal { get; set; }

    public bool Suppres { get; set; }

    public string? Remarks { get; set; }

    public string CatCode { get; set; } = null!;

    public string CatType { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string AcctDebit { get; set; } = null!;

    public string? AcctCredit { get; set; }

    public DateTime? ExpDate { get; set; }

    public bool? IsPost { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
