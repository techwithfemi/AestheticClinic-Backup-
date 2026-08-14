using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hScreeningAmount")]
public partial class hScreeningAmount
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CoyCode { get; set; } = null!;

    public double Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ScreenName { get; set; } = null!;

    [MaxLength(150)]
    public byte[] Remarks { get; set; } = null!;
}
