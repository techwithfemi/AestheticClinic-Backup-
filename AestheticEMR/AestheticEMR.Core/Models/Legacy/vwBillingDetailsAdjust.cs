using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetailsAdjust
{
    [Column(TypeName = "datetime")]
    public DateTime AdjustDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [StringLength(100)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string BillNo { get; set; } = null!;

    [StringLength(251)]
    public string Fullname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string BillItem { get; set; } = null!;

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OldQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal NewQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OldPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal NewPrice { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? OldAmount { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? NewAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
