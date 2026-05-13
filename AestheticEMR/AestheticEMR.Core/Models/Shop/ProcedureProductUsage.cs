using AestheticEMR.Core.Models.Aesthetic;

namespace AestheticEMR.Core.Models.Shop
{
    public class ProcedureProductUsage : BaseEntity
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int ProductBatchId { get; set; }
        public required ProductBatch ProductBatch { get; set; }

        public int ConsultationId { get; set; }
        public required AestheticConsultation Consultation { get; set; }

        public required string ProcedureType { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime UsedOn { get; set; }
        public string? Notes { get; set; }
    }
}