using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhReferalForEdit
{
    public long Id { get; set; }

    public string? PNo { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public string? ReferTo { get; set; }

    public string? RefReason { get; set; }

    public DateTime? RefDate { get; set; }

    public DateTime? RefTime { get; set; }

    public bool? AttendedTo { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? RefAddress { get; set; }

    public string? Coyname { get; set; }

    public bool? Suppres { get; set; }

    public string? TreatedBy { get; set; }

    public string? ReferedBy { get; set; }

    public bool? AttendedToByRec { get; set; }

    public string Company { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? ConId { get; set; }
}
