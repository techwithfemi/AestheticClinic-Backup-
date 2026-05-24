using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForClinic
{
    public string PNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public int RecId { get; set; }

    public string? RoomNo { get; set; }

    public bool? Suppres { get; set; }

    public string ClinicType { get; set; } = null!;
}
