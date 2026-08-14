using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillHeadingWithNull
{
    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? BillHead { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    public int? SNO { get; set; }

    public double subTotal { get; set; }
}
