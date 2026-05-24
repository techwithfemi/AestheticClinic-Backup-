using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillInfoMgt
{
    public string BillNo { get; set; } = null!;

    public double AmountGen { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public DateTime AttdDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? Remarks { get; set; }

    public string? ClientCatId { get; set; }
}
