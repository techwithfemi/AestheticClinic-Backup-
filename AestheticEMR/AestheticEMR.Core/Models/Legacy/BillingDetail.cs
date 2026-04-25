using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class BillingDetail
{
    public string billNO { get; set; } = null!;

    public long SNO { get; set; }

    public DateTime dtDate { get; set; }

    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? subTotal { get; set; }

    public string? billType { get; set; }

    public string? conID { get; set; }

    public string? Capitated { get; set; }

    public string? Dosage { get; set; }

    public string? Category { get; set; }

    public string BillTo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? BillHead { get; set; }

    public string? revType { get; set; }

    public string? DRGCode { get; set; }

    public bool isPost { get; set; }

    public bool? isRct { get; set; }

    public string? BillBy { get; set; }

    public string? treatedBy { get; set; }

    public string? Dept { get; set; }

    public bool? isOLD { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? RevClinic { get; set; }

    public decimal? AmtPaid { get; set; }

    public bool? Reversed { get; set; }

    public string? Remarks { get; set; }

    public bool? suppres { get; set; }

    public int? AppVersion { get; set; }

    public string? TranID { get; set; }

    public long? ReversedPair { get; set; }
}
