using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryInvoiceList
{
    public string SupplierName { get; set; } = null!;

    public DateTime InvoiceDate { get; set; }

    public string? InvoiceNo { get; set; }

    public long SupplierId { get; set; }

    public string OrderNo { get; set; } = null!;

    public long Sno { get; set; }
}
