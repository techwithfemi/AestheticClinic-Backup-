using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("OrderId", Name = "IX_AppOrderDetails_OrderId")]
[Index("ProductId", Name = "IX_AppOrderDetails_ProductId")]
public partial class AppOrderDetail
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("AppOrderDetails")]
    public virtual AppOrder Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("AppOrderDetails")]
    public virtual AppProduct Product { get; set; } = null!;
}
