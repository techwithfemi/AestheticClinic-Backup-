using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class OrderDetail
{
    public int? OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public float Discount { get; set; }

    public string? PriceMethod { get; set; }

    public string? Product { get; set; }
}
