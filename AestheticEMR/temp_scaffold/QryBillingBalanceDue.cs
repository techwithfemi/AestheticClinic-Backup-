using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingBalanceDue
{
    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string ClientId { get; set; } = null!;

    public string Clientname { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? AmountDue { get; set; }
}
