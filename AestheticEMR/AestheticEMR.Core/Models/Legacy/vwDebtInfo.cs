using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDebtInfo
{
    public long SNO { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Debt { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(251)]
    [Unicode(false)]
    public string? FullName { get; set; }
}
