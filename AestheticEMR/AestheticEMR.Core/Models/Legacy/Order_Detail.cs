using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Order Details")]
public partial class Order_Detail
{
    public int? OrderID { get; set; }

    public int ProductID { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public float Discount { get; set; }

    [StringLength(50)]
    public string? PriceMethod { get; set; }

    [StringLength(50)]
    public string? Product { get; set; }
}
