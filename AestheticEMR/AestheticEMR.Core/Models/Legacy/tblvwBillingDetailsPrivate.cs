using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("tblvwBillingDetailsPrivate")]
public partial class tblvwBillingDetailsPrivate
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string DrgName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    public double Price { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }
}
