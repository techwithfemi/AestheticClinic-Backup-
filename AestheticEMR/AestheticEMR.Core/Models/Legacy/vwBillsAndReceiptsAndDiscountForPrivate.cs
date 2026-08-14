using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsAndReceiptsAndDiscountForPrivate
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string Billno { get; set; } = null!;

    public double? Amount { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string Remarks2 { get; set; } = null!;

    public int Seed { get; set; }
}
