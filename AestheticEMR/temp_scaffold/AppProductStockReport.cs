using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppProductStockReport
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string OperationType { get; set; } = null!;

    public decimal BuyingPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal PreviousBuyingPrices { get; set; }

    public decimal PreviousSellingPrice { get; set; }

    public int PreviousUnitsInStock { get; set; }

    public int UnitsInStock { get; set; }

    public DateTime OperationDate { get; set; }

    public DateTime OperationTime { get; set; }

    public DateTime OperationTimestamp { get; set; }

    public string? UserName { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AppProduct Product { get; set; } = null!;
}
