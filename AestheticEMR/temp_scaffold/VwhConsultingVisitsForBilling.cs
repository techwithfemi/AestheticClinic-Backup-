using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingVisitsForBilling
{
    public string Fullname { get; set; } = null!;

    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string Client { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string ConsultId { get; set; } = null!;

    public double? Debt { get; set; }

    public bool? IsBilled { get; set; }

    public string Clinic { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? ClientCatId { get; set; }

    public string PNo { get; set; } = null!;
}
