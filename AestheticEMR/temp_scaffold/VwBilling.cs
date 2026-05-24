using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBilling
{
    public DateTime Date { get; set; }

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? PCatId { get; set; }

    public decimal? AmtCf { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string? AmountBilledInWord { get; set; }

    public string BillNo { get; set; } = null!;

    public bool? IsPaid { get; set; }

    public string? ClientCatId { get; set; }
}
