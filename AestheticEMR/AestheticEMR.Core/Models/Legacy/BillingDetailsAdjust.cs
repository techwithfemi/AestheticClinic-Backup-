using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BillingDetailsAdjust")]
public partial class BillingDetailsAdjust
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdjustDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdjustTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BillNo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string BillItem { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OldQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal NewQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OldPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal NewPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AdjustBy { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
