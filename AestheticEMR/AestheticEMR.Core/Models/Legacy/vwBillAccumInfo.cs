using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillAccumInfo
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public bool? isBilled { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(30)]
    public string? Ref { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    public double subTotal { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }
}
