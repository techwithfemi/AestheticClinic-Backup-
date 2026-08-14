using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillAccumUnion
{
    public int? SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? billtype { get; set; }
}
