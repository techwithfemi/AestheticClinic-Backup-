using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDelivery
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime DelvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string NameOfDoctor { get; set; } = null!;

    public string NameOfNurse { get; set; } = null!;

    public string DelvOutcome { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? Dob { get; set; }

    public DateTime? Tob { get; set; }

    public string? ApgarScore { get; set; }

    public decimal? Wt { get; set; }

    public decimal? BirthLength { get; set; }

    public decimal? HeadCircumference { get; set; }

    public string? Rbs { get; set; }

    public string? InjectionTaken { get; set; }

    public string? ImmunizationDone { get; set; }
}
