using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsForClients2
{
    public string? BatchNo { get; set; }

    public DateTime? InvDate { get; set; }

    public string? InvNo { get; set; }

    public string? CoyCode { get; set; }

    public DateTime DtDate { get; set; }

    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string? Period { get; set; }

    public string? BillYear { get; set; }

    public string? BillMonth { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public decimal? AmountBilled { get; set; }

    public string BillNo { get; set; } = null!;

    public bool? IsPost { get; set; }

    public string? BillHead { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBf { get; set; }

    public string? Diagnosis { get; set; }

    public long Sno { get; set; }
}
