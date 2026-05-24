using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public decimal BuyingPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public int UnitsInStock { get; set; }

    public bool IsActive { get; set; }

    public bool IsDiscontinued { get; set; }

    public int? ParentId { get; set; }

    public int ProductCategoryId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal PreviousBuyingPrices { get; set; }

    public decimal PreviousSellingPrice { get; set; }

    public int PreviousUnitsInStock { get; set; }

    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; set; } = new List<AppOrderDetail>();

    public virtual ICollection<AppProcedureProductUsage> AppProcedureProductUsages { get; set; } = new List<AppProcedureProductUsage>();

    public virtual ICollection<AppProductBatch> AppProductBatches { get; set; } = new List<AppProductBatch>();

    public virtual ICollection<AppProductStockReport> AppProductStockReports { get; set; } = new List<AppProductStockReport>();

    public virtual ICollection<AppProduct> InverseParent { get; set; } = new List<AppProduct>();

    public virtual AppProduct? Parent { get; set; }

    public virtual AppProductCategory ProductCategory { get; set; } = null!;
}
