using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOApproved2
{
    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    [StringLength(50)]
    public string? Drug { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    public double? QtyInStock { get; set; }

    public double Qty { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string POID { get; set; } = null!;

    [StringLength(50)]
    public string? ItemName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Address { get; set; } = null!;
}
