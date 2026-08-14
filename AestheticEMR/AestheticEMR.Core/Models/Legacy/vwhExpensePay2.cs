using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpensePay2
{
    [Column(TypeName = "datetime")]
    public DateTime ExpDate { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(150)]
    public string? ReceivedBy { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(101)]
    public string? PaidBy { get; set; }

    [StringLength(101)]
    public string? ApprvBy { get; set; }
}
