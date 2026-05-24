using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClientBill
{
    public DateTime BDate { get; set; }

    public string Fullname { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public string? ClientId { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public string ClientName { get; set; } = null!;
}
