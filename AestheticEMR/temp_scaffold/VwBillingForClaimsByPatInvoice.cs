using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForClaimsByPatInvoice
{
    public int Sno { get; set; }

    public double? SubTotal { get; set; }

    public string BillNo { get; set; } = null!;

    public string? BilltRemarks { get; set; }

    public string Fullname { get; set; } = null!;

    public DateTime? DischDate { get; set; }

    public DateTime? AdmDate { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? Age { get; set; }

    public string? BillToNo { get; set; }

    public string Company { get; set; } = null!;

    public decimal AmountPaid { get; set; }
}
