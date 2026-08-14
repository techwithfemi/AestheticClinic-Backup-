using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsAndDetail
{
    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }
}
