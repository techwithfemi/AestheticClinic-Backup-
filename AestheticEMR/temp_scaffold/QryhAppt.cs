using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAppt
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public DateTime EntryTime { get; set; }

    public DateTime ApptDate { get; set; }

    public DateTime ApptTime { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }
}
