using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsFromAttndOffline
{
    public int RecId { get; set; }

    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? AttendedToByDoc { get; set; }

    public DateTime RecDate { get; set; }

    public DateTime? Htime { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? AttendedToByNurse { get; set; }

    public bool? Suppres { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }
}
