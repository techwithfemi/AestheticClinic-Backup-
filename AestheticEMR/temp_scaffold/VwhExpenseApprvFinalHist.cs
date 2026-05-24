using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseApprvFinalHist
{
    public string VouchNo { get; set; } = null!;

    public long ExpId { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public DateTime? ExpDate { get; set; }

    public string? Description { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? SubTotal { get; set; }

    public string? PersName { get; set; }

    public string? ApprvBy { get; set; }
}
