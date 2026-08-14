using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntryInvoiceList
{
    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime InvoiceDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? InvoiceNo { get; set; }

    public long SupplierID { get; set; }

    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    public long SNo { get; set; }
}
