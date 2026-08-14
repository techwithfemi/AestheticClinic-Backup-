using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillAccumUnion
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }

    public double AmountBilled { get; set; }
}
