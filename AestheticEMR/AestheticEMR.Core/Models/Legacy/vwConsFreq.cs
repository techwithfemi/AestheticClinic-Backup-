using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwConsFreq
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    public int? ConsFreq { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountBilled { get; set; }

    public double Capitation { get; set; }

    [StringLength(30)]
    public string? AttndMth { get; set; }

    [StringLength(30)]
    public string? AttndYr { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Period { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }
}
