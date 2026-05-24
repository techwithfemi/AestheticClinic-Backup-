using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhBillingBalanceForPrivate
{
    public DateTime BDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? HomeAddress { get; set; }

    public string? PPhoneNo { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string? CoyName { get; set; }

    public bool? IsPaid { get; set; }

    public string Fullname { get; set; } = null!;

    public string? AmountBilledInWord { get; set; }

    public string? ClientCatId { get; set; }

    public string? RetainCode { get; set; }
}
