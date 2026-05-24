using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Customer
{
    public string CustId { get; set; } = null!;

    public string? CustName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public string? ContactTitle { get; set; }
}
