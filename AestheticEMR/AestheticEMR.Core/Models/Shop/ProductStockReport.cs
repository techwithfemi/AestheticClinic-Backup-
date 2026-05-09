// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace AestheticEMR.Core.Models.Shop
{
    public class ProductStockReport : BaseEntity
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public required string OperationType { get; set; }
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
}
