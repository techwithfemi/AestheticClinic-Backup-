namespace AestheticEMR.Core.Services.Legacy.Models;

public sealed class ShiftLookupItem
{
    public long ShiftId { get; set; }
    public string ShiftJob { get; set; } = string.Empty;
}

public sealed class ShiftDetailItem
{
    public long ShiftId { get; set; }
    public string ShiftJob { get; set; } = string.Empty;
    public string PeriodOfDay { get; set; } = string.Empty;
    public string? ResumptionTime { get; set; }
    public string? ClosingTime { get; set; }
    public string? PunctualityRemarks { get; set; }
    public string? LateRemarks { get; set; }
    public string? NormalClosingRemarks { get; set; }
    public string? AbnormalClosingRemarks { get; set; }
    public string? EvalTo { get; set; }
}
