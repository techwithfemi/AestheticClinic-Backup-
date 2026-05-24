using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateAmountForLab
{
    public string Fullname { get; set; } = null!;

    public string? Billtype { get; set; }

    public string Company { get; set; } = null!;

    public string? DrgName { get; set; }

    public decimal? Amount { get; set; }

    public DateTime InvDate { get; set; }

    public decimal? AmountAccum { get; set; }

    public string BillNo { get; set; } = null!;

    public string? Coyname { get; set; }

    public string? Capitated { get; set; }

    public string? SympItemCat { get; set; }

    public decimal? Price { get; set; }

    public decimal? Qty { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ConId { get; set; } = null!;

    public decimal? Cost { get; set; }

    public long? LabItemSno { get; set; }

    public string? LabNum { get; set; }

    public string? RevType { get; set; }

    public string? BillType2 { get; set; }
}
