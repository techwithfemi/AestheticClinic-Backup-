using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHdiagnosisStat
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Sex { get; set; }

    public string? Symptoms { get; set; }

    public string? Complaints { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? Diagnosis2 { get; set; }

    public DateTime? Dob { get; set; }

    public int? Age { get; set; }

    public int? AgeInMths { get; set; }

    public int? AgeInDays { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClinicType { get; set; } = null!;
}
