using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("CashierId", Name = "IX_AppOrders_CashierId")]
[Index("CustomerId", Name = "IX_AppOrders_CustomerId")]
public partial class AppOrder
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    public string? CashierId { get; set; }

    public int CustomerId { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; set; } = new List<AppOrderDetail>();

    [ForeignKey("CashierId")]
    [InverseProperty("AppOrders")]
    public virtual AspNetUser? Cashier { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("AppOrders")]
    public virtual AppCustomer Customer { get; set; } = null!;
}
