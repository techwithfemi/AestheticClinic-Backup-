using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntryInvoice
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string? ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Drug { get; set; }

    [StringLength(150)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    [StringLength(50)]
    public string? OrderNo { get; set; }

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(60)]
    public string Address { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime InvoiceDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? InvoiceNo { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    public long SupplierID { get; set; }
}
