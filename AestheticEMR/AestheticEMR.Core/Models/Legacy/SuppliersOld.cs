using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("SuppliersOld")]
public partial class SuppliersOld
{
    public long SupplierID { get; set; }

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    public double? Credit { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public bool? initBal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? debt { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CatCode { get; set; }

    public bool? isPost { get; set; }
}
