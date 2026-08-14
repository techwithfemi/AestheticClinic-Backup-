using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("Name", Name = "IX_AppCustomers_Name")]
public partial class AppCustomer
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    public int Gender { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<AppOrder> AppOrders { get; set; } = new List<AppOrder>();
}
