using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsForClientsPostDetail
{
    public int? Sno { get; set; }

    public long SnoInv { get; set; }

    public DateTime Date { get; set; }

    public DateTime BDate { get; set; }

    public DateTime InvDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public string? BatchNo { get; set; }

    public string? BatchVal { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public bool? IsPost { get; set; }

    public string Posted { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? RevType { get; set; }

    public string? Debit { get; set; }

    public string? Credit { get; set; }

    public double Amount { get; set; }

    public string DrgName { get; set; } = null!;
}
