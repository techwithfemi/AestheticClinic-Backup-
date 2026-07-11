using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public sealed class ShiftLookupVM
{
    public long ShiftId { get; set; }
    public string ShiftJob { get; set; } = string.Empty;
}

public sealed class ShiftDetailVM
{
    public long ShiftId { get; set; }

    [Required, StringLength(200)]
    public string ShiftJob { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string PeriodOfDay { get; set; } = string.Empty;

    [Required, StringLength(5)]
    public string ResumptionTime { get; set; } = string.Empty;

    [Required, StringLength(5)]
    public string ClosingTime { get; set; } = string.Empty;

    [StringLength(200)]
    public string? PunctualityRemarks { get; set; }

    [StringLength(200)]
    public string? LateRemarks { get; set; }

    [StringLength(200)]
    public string? NormalClosingRemarks { get; set; }

    [StringLength(200)]
    public string? AbnormalClosingRemarks { get; set; }

    [StringLength(50)]
    public string? EvalTo { get; set; }
}
