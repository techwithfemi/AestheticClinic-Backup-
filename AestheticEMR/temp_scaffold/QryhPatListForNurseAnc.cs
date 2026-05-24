using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPatListForNurseAnc
{
    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? ClientCat { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public string? Remarks { get; set; }

    public string ClinicType { get; set; } = null!;
}
