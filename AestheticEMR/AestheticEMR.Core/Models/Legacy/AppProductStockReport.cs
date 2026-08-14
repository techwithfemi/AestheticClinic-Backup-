using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("ProductId", Name = "IX_AppProductStockReports_ProductId")]
public partial class AppProductStockReport
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [StringLength(20)]
    public string OperationType { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal BuyingPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SellingPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PreviousBuyingPrices { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PreviousSellingPrice { get; set; }

    public int PreviousUnitsInStock { get; set; }

    public int UnitsInStock { get; set; }

    public DateTime OperationDate { get; set; }

    public DateTime OperationTime { get; set; }

    public DateTime OperationTimestamp { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("AppProductStockReports")]
    public virtual AppProduct Product { get; set; } = null!;
}
