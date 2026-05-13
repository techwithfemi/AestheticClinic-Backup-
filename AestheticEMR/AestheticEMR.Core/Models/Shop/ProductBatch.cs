namespace AestheticEMR.Core.Models.Shop
{
    public class ProductBatch : BaseEntity
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public required string BatchNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityRemaining { get; set; }
        public bool IsRecalled { get; set; }
        public DateTime? RecalledOn { get; set; }
        public string? RecallReason { get; set; }

        public ICollection<ProcedureProductUsage> ProcedureUsages { get; } = [];
    }
}