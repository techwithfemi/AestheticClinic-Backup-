using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("Name", Name = "IX_AppProducts_Name")]
[Index("ParentId", Name = "IX_AppProducts_ParentId")]
[Index("ProductCategoryId", Name = "IX_AppProducts_ProductCategoryId")]
public partial class AppProduct
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Unicode(false)]
    public string? Icon { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal BuyingPrice { get; set; }

    public int UnitsInStock { get; set; }

    public bool IsActive { get; set; }

    public bool IsDiscontinued { get; set; }

    public int? ParentId { get; set; }

    public int ProductCategoryId { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PreviousBuyingPrices { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PreviousSellingPrice { get; set; }

    public int PreviousUnitsInStock { get; set; }

    public string? LastInventoryTranID { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; set; } = new List<AppOrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<AppProcedureProductUsage> AppProcedureProductUsages { get; set; } = new List<AppProcedureProductUsage>();

    [InverseProperty("Product")]
    public virtual ICollection<AppProductBatch> AppProductBatches { get; set; } = new List<AppProductBatch>();

    [InverseProperty("Product")]
    public virtual ICollection<AppProductStockReport> AppProductStockReports { get; set; } = new List<AppProductStockReport>();

    [InverseProperty("Parent")]
    public virtual ICollection<AppProduct> InverseParent { get; set; } = new List<AppProduct>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual AppProduct? Parent { get; set; }

    [ForeignKey("ProductCategoryId")]
    [InverseProperty("AppProducts")]
    public virtual AppProductCategory ProductCategory { get; set; } = null!;
}
