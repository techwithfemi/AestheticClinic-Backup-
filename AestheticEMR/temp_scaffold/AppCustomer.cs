using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppCustomer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public int Gender { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AppOrder> AppOrders { get; set; } = new List<AppOrder>();
}
