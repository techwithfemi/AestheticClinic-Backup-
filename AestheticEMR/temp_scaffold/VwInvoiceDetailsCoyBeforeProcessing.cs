using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoyBeforeProcessing
{
    public DateTime Date { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public string CoyCode { get; set; } = null!;

    public string? ClientCat { get; set; }

    public decimal? AmountBilled { get; set; }

    public string BillNo { get; set; } = null!;

    public string? InvNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public int? NoOfDays { get; set; }

    public string? BillHead { get; set; }

    public DateTime? BillDate { get; set; }
}
