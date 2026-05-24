using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppOrderDetail
{
    public int Id { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Discount { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AppOrder Order { get; set; } = null!;

    public virtual AppProduct Product { get; set; } = null!;
}
