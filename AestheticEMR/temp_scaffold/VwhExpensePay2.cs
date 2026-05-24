using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpensePay2
{
    public DateTime ExpDate { get; set; }

    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    public string? ReceivedBy { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? PaidBy { get; set; }

    public string? ApprvBy { get; set; }
}
