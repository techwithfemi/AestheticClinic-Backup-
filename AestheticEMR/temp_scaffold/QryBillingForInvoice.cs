using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingForInvoice
{
    public DateTime BDate { get; set; }

    public DateTime ConsultDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public decimal AmountBilled { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal AmountPaid { get; set; }

    public string? CoyName { get; set; }

    public string? PCatId { get; set; }

    public string? DrgCatGroup { get; set; }

    public string? CatRemarks { get; set; }
}
