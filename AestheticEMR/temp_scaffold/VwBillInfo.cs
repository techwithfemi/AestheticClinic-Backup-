using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillInfo
{
    public string ConsultId { get; set; } = null!;

    public decimal AmountGen { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal AmountBilled2 { get; set; }

    public decimal? AmountPayable { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? AmountBal { get; set; }

    public decimal? AmountCap { get; set; }

    public decimal Discount { get; set; }

    public decimal Debt { get; set; }

    public decimal Debt2 { get; set; }
}
