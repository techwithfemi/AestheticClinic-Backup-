using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsAndDetail
{
    public string Fullname { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? SubTotal { get; set; }
}
