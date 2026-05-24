using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxaction
{
    public long Sno { get; set; }

    public DateTime AttndDate { get; set; }

    public DateTime? BillDate { get; set; }

    public string FullName { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? Bill { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? DebtBf { get; set; }

    public decimal? Discount { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public int RunningTotal { get; set; }

    public string Remarks { get; set; } = null!;

    public string Comapany { get; set; } = null!;

    public decimal? Debt { get; set; }

    public string? ClientId { get; set; }

    public string? Diagnosis { get; set; }

    public string? PhoneNo { get; set; }

    public string RetainCode { get; set; } = null!;
}
