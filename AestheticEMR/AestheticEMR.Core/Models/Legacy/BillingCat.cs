using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BillingCat")]
public partial class BillingCat
{
    [StringLength(50)]
    public string clientcatID { get; set; } = null!;

    [StringLength(50)]
    public string? MapTo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? pCent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? pvalue { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }

    public bool? sysVal { get; set; }

    public bool? isCustom { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Capitated { get; set; }

    public bool? isReserved { get; set; }
}
