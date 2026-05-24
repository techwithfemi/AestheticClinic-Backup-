using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHdelivery
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime DelvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string Doctor { get; set; } = null!;

    public string NameOfNurse { get; set; } = null!;

    public string? Remarks { get; set; }

    public string DelvOutcome { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string DocId { get; set; } = null!;

    public DateTime? Tob { get; set; }

    public string? As { get; set; }

    public decimal? Wt { get; set; }

    public decimal? Bl { get; set; }

    public decimal? Hc { get; set; }

    public string? Rbs { get; set; }

    public string? Injections { get; set; }

    public string? Immunizations { get; set; }

    public DateTime Dob { get; set; }
}
