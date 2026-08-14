using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseApprvFinalHist
{
    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    public long ExpID { get; set; }

    [StringLength(7)]
    public string ItemCode { get; set; } = null!;

    [StringLength(255)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SubTotal { get; set; }

    [StringLength(101)]
    public string? PersName { get; set; }

    [StringLength(101)]
    public string? ApprvBy { get; set; }
}
