using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDebtByPNoSumm
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Balance { get; set; }

    [StringLength(150)]
    public string? clientID { get; set; }
}
