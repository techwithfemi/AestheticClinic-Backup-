using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Shipper
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }
}
