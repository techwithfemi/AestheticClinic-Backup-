namespace AestheticEMR.Core.Models.Aesthetic
{
    public class ProcedureRevenueMetric
    {
        public string ProcedureType { get; set; } = string.Empty;
        public int ConsultationCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ProductUsageMetric
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantityUsed { get; set; }
    }

    public class ComplicationRateMetric
    {
        public int TotalCompletedFollowUps { get; set; }
        public int ComplicationCases { get; set; }
        public decimal ComplicationRatePercent { get; set; }
    }

    public class PatientRetentionMetric
    {
        public int TotalPatients { get; set; }
        public int ReturningPatients { get; set; }
        public decimal RetentionRatePercent { get; set; }
    }

    public class BeforeAfterOutcomeMetric
    {
        public int TotalConsultationsWithPhotos { get; set; }
        public int ConsultationsWithBeforeAfter { get; set; }
        public decimal BeforeAfterRatePercent { get; set; }
    }
}