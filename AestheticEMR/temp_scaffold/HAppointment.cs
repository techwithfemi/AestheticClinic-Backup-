using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAppointment
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string? ConsultId { get; set; }

    public string? ClientCat { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ClinicType { get; set; }

    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? RetainCode { get; set; }

    public string? EmpId { get; set; }

    public bool? AttendedToByRec { get; set; }
}
