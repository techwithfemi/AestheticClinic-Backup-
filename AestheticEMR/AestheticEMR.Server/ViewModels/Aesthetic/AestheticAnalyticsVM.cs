namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class ProcedureRevenueMetricVM
    {
        public string? ProcedureType { get; set; }
        public int ConsultationCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ProductUsageMetricVM
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalQuantityUsed { get; set; }
    }

    public class ComplicationRateMetricVM
    {
        public int TotalCompletedFollowUps { get; set; }
        public int ComplicationCases { get; set; }
        public decimal ComplicationRatePercent { get; set; }
    }

    public class PatientRetentionMetricVM
    {
        public int TotalPatients { get; set; }
        public int ReturningPatients { get; set; }
        public decimal RetentionRatePercent { get; set; }
    }

    public class BeforeAfterOutcomeMetricVM
    {
        public int TotalConsultationsWithPhotos { get; set; }
        public int ConsultationsWithBeforeAfter { get; set; }
        public decimal BeforeAfterRatePercent { get; set; }
    }
}