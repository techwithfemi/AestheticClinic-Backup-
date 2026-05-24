using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillAccumUnion
{
    public DateTime Date { get; set; }

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Service { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }

    public double AmountBilled { get; set; }
}
