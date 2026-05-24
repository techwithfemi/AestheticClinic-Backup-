using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPreConsultArchive
{
    public int Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public string? Remarks { get; set; }

    public string ExaminedBy { get; set; } = null!;

    public DateTime? PreTime { get; set; }

    public string? Temp { get; set; }

    public string? Pressure { get; set; }

    public string? Pulse { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? UrineAlb { get; set; }

    public string? UrineSug { get; set; }

    public string? RespRatio { get; set; }

    public string ClientCat { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? Stool { get; set; }

    public string? Urine { get; set; }

    public string? Sdrainage { get; set; }

    public string? Status { get; set; }

    public string? Comment { get; set; }
}
