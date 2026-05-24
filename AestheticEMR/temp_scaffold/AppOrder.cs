using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppOrder
{
    public int Id { get; set; }

    public decimal Discount { get; set; }

    public string? Comments { get; set; }

    public string? CashierId { get; set; }

    public int CustomerId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; set; } = new List<AppOrderDetail>();

    public virtual AspNetUser? Cashier { get; set; }

    public virtual AppCustomer Customer { get; set; } = null!;
}
