using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPreconsultEdit
{
    public long Id { get; set; }

    public long? ConId { get; set; }

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public string? Remarks { get; set; }

    public string? ExaminedBy { get; set; }

    public DateTime? PreTime { get; set; }

    public string? Temp { get; set; }

    public string? Pressure { get; set; }

    public string? Pulse { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? UrineAlb { get; set; }

    public string? UrineSug { get; set; }

    public string? RespRatio { get; set; }

    public string? ClientCat { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? Occupation { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Status { get; set; }

    public string? Stool { get; set; }

    public string? Urine { get; set; }

    public string? Sdrainage { get; set; }

    public string? Comment { get; set; }

    public string? Company { get; set; }

    public string? Referal { get; set; }

    public DateTime? Dob { get; set; }

    public string? Coyname { get; set; }

    public string RetainCode { get; set; } = null!;

    public string? Expr1 { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? Spo2 { get; set; }

    public string? Rbs { get; set; }

    public string? Positioning { get; set; }

    public string? Nurse { get; set; }

    public string? Tb { get; set; }
}
