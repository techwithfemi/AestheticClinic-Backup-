using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPreconsultForGrid
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? Temp { get; set; }

    public string? Pressure { get; set; }

    public string? Pulse { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? RespRatio { get; set; }

    public string? ClientCat { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Details { get; set; }

    public string? EmpId { get; set; }

    public string? Stool { get; set; }

    public string? Urine { get; set; }

    public string? Sdrainage { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Comment { get; set; }

    public string? Examinedby { get; set; }

    public string? UrineAlb { get; set; }

    public string? UrineSug { get; set; }

    public string? Status { get; set; }

    public string? Nurse { get; set; }

    public string? DocAssigned { get; set; }

    public string? AssignedTo { get; set; }

    public string? Spo2 { get; set; }

    public string? Rbs { get; set; }

    public string? Positioning { get; set; }

    public string? Tb { get; set; }
}
