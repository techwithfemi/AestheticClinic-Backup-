using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillDetailsSummByType
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountAccum { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType2 { get; set; }
}
