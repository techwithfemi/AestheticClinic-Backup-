using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForLabCombo
{
    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ClientCat { get; set; }

    public string Remarks { get; set; } = null!;

    public int? Age { get; set; }

    public string CoyCode { get; set; } = null!;

    public string? Coyname { get; set; }

    public long Id { get; set; }

    public DateTime? CDate { get; set; }

    public DateTime? CTime { get; set; }

    public bool? Suppres { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public string? Ref { get; set; }

    public bool? AttendedTo { get; set; }

    public DateTime InvDate { get; set; }

    public string Fullname { get; set; } = null!;
}
