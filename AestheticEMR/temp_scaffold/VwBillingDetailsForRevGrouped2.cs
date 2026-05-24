using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsForRevGrouped2
{
    public DateTime Date { get; set; }

    public DateTime? BillDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string BillNo { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? ApprvCode { get; set; }
}
