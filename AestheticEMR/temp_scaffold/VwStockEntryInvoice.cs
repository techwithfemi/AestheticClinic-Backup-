using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryInvoice
{
    public long Sno { get; set; }

    public string? Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Drug { get; set; }

    public string? Category { get; set; }

    public decimal Qty { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? Amount { get; set; }

    public string? Poid { get; set; }

    public string? ItemName { get; set; }

    public string? OrderNo { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime InvoiceDate { get; set; }

    public string? InvoiceNo { get; set; }

    public string? LocId { get; set; }

    public long SupplierId { get; set; }
}
