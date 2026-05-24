using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAdmission
{
    public long Sno { get; set; }

    public DateTime AdmDate { get; set; }

    public string PNo { get; set; } = null!;

    public string? WardId { get; set; }

    public string? Reason { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ATime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public bool? IsDischarged { get; set; }

    public string? AdmitBy { get; set; }

    public bool? IsDischargedByDoc { get; set; }

    public string? AdmitingDoc { get; set; }

    public DateTime? DischDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? DocInCharge { get; set; }

    public long? ConId { get; set; }

    public int? NoOfDaysAdmission { get; set; }

    public int? AdmissionDaysAllowed { get; set; }

    public DateTime? ExtendAdmissionLimitTo { get; set; }

    public string? Comment { get; set; }
}
