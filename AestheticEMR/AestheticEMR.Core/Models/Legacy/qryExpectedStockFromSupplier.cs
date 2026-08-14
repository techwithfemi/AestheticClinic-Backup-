using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryExpectedStockFromSupplier
{
    [Column("LPO No")]
    [StringLength(50)]
    public string LPO_No { get; set; } = null!;

    [Column("Supplier No")]
    [StringLength(50)]
    public string? Supplier_No { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ExpectedDate { get; set; }

    [StringLength(50)]
    public string? ItemCode { get; set; }

    [StringLength(50)]
    public string ItemName { get; set; } = null!;
}
