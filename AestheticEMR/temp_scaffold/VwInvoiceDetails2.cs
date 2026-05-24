using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetails2
{
    public DateTime Date { get; set; }

    public string PNo { get; set; } = null!;

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string InvNo { get; set; } = null!;

    public string CoyCode { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string? ClientCat { get; set; }

    public string? Period { get; set; }

    public string? BillYear { get; set; }

    public string? BillMonth { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public decimal? AmountBilled { get; set; }

    public string BillNo { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string? BillHead { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBf { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime? BillDate { get; set; }

    public string? BatchNo2 { get; set; }

    public string RetainCode { get; set; } = null!;

    public long Sno { get; set; }

    public string? CoyId { get; set; }
}
