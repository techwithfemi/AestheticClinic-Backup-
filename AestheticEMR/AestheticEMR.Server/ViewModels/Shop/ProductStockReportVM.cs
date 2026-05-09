namespace AestheticEMR.Server.ViewModels.Shop;

public class ProductStockReportVM
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }

    public string? OperationType { get; set; }
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
}
