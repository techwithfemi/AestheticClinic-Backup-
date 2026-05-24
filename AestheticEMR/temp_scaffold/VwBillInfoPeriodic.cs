using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillInfoPeriodic
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public double? AmountGen { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string RetainId { get; set; } = null!;
}
