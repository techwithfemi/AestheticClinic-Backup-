using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatientsForBilling
{
    public string PNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public int RecId { get; set; }

    public bool? Suppres { get; set; }

    public string ClinicType { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? Company { get; set; }

    public string? Remarks { get; set; }

    public string? ClientCat { get; set; }

    public int? Age { get; set; }

    public string Sex { get; set; } = null!;

    public string? Occupation { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Status { get; set; }
}
