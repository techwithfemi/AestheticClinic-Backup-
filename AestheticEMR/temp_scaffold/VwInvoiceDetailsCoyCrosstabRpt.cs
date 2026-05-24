using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoyCrosstabRpt
{
    public DateTime? BillDate { get; set; }

    public string Fullname { get; set; } = null!;

    public int? NoOfDays { get; set; }

    public string? BatchNo { get; set; }

    public string? CoyCode { get; set; }

    public string Company { get; set; } = null!;

    public string InvNo { get; set; } = null!;

    public decimal? Consultation { get; set; }

    public decimal? Drug { get; set; }

    public decimal? Injection { get; set; }

    public decimal? InfusionTransfusion { get; set; }

    public decimal? Lab { get; set; }

    public decimal? Scan { get; set; }

    public decimal? Ecg { get; set; }

    public decimal? XRay { get; set; }

    public decimal? Procedure { get; set; }

    public decimal? Admission { get; set; }

    public decimal? Dental { get; set; }

    public decimal? Eye { get; set; }

    public decimal? MSurgery { get; set; }

    public decimal? Feeding { get; set; }
}
