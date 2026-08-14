using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Customer
{
    [StringLength(50)]
    public string CustID { get; set; } = null!;

    [StringLength(150)]
    public string? CustName { get; set; }

    [StringLength(100)]
    public string? Address { get; set; }

    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? Contact { get; set; }

    [StringLength(50)]
    public string? ContactTitle { get; set; }
}
