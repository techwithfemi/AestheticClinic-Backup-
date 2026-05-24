using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoy2ForUnProcessed
{
    public DateTime? BillDate { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public string InvNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? BatchNo2 { get; set; }

    public string CoyCode { get; set; } = null!;

    public DateTime? Date { get; set; }
}
