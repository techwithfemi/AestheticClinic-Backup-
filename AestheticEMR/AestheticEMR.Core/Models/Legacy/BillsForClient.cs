using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class BillsForClient
{
    public long SNO { get; set; }

    public DateTime InvDate { get; set; }

    public string InvNo { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? AmtBF { get; set; }

    public string? AmountBilledInWord { get; set; }

    public string? BillMonth { get; set; }

    public string? BillYear { get; set; }

    public bool? isPaid { get; set; }

    public string? BatchNo { get; set; }

    public bool? isOLd { get; set; }

    public bool? isPost { get; set; }

    public bool? AttendedToByClient { get; set; }
}
