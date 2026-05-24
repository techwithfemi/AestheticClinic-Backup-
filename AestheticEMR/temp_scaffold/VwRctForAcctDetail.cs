using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwRctForAcctDetail
{
    public long Sno { get; set; }

    public DateTime BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string BillItem { get; set; } = null!;

    public decimal? SubTotal { get; set; }

    public string? RevType { get; set; }

    public string AccountNo { get; set; } = null!;

    public bool? IsRct { get; set; }
}
