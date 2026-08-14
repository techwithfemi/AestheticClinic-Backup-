using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhpaymentsSummDepositPublic
{
    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}
