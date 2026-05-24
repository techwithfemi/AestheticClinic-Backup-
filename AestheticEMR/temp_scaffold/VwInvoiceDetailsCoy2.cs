using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoy2
{
    public DateTime Date { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal? AubTotal { get; set; }

    public string InvNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal? AmountBilled { get; set; }

    public bool? IsPost { get; set; }

    public string? BillYear { get; set; }

    public string? BillMonth { get; set; }

    public string? Period { get; set; }

    public string? AcctId { get; set; }
}
