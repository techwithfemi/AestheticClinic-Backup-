using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillsForClient
{
    public long Sno { get; set; }

    public DateTime InvDate { get; set; }

    public string InvNo { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? AmtBf { get; set; }

    public string? AmountBilledInWord { get; set; }

    public string? BillMonth { get; set; }

    public string? BillYear { get; set; }

    public bool? IsPaid { get; set; }

    public string? BatchNo { get; set; }

    public bool? IsOld { get; set; }

    public bool? IsPost { get; set; }

    public bool? AttendedToByClient { get; set; }
}
