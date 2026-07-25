using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HDentalTreat
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Dtype { get; set; }

    public DateTime TDate { get; set; }

    public DateTime TTime { get; set; }

    public string? TeethStatusJson { get; set; }
    public string? OrthodonticsJson { get; set; }
    public string? OralExamJson { get; set; }

    public bool? Auli1 { get; set; }

    public bool? Auli2 { get; set; }

    public bool? Aulc { get; set; }

    public bool? Aulpm1 { get; set; }

    public bool? Aulpm2 { get; set; }

    public bool? Aulm1 { get; set; }

    public bool? Aulm2 { get; set; }

    public bool? Aulm3 { get; set; }

    public bool? Auri1 { get; set; }

    public bool? Auri2 { get; set; }

    public bool? Aurc { get; set; }

    public bool? Aurpm1 { get; set; }

    public bool? Aurpm2 { get; set; }

    public bool? Aurm1 { get; set; }

    public bool? Aurm2 { get; set; }

    public bool? Aurm3 { get; set; }

    public bool? Alli1 { get; set; }

    public bool? Alli2 { get; set; }

    public bool? Allc { get; set; }

    public bool? Allpm1 { get; set; }

    public bool? Allpm2 { get; set; }

    public bool? Allm1 { get; set; }

    public bool? Allm2 { get; set; }

    public bool? Allm3 { get; set; }

    public bool? Alri1 { get; set; }

    public bool? Alri2 { get; set; }

    public bool? Alrc { get; set; }

    public bool? Alrpm1 { get; set; }

    public bool? Alrpm2 { get; set; }

    public bool? Alrm1 { get; set; }

    public bool? Alrm2 { get; set; }

    public bool? Alrm3 { get; set; }

    public bool? Culi1 { get; set; }

    public bool? Culi2 { get; set; }

    public bool? Culc { get; set; }

    public bool? Culpm1 { get; set; }

    public bool? Culpm2 { get; set; }

    public bool? Curi1 { get; set; }

    public bool? Curi2 { get; set; }

    public bool? Curc { get; set; }

    public bool? Curpm1 { get; set; }

    public bool? Curpm2 { get; set; }

    public bool? Clli1 { get; set; }

    public bool? Clli2 { get; set; }

    public bool? Cllc { get; set; }

    public bool? Cllpm1 { get; set; }

    public bool? Cllpm2 { get; set; }

    public bool? Clri1 { get; set; }

    public bool? Clri2 { get; set; }

    public bool? Clrc { get; set; }

    public bool? Clrpm1 { get; set; }

    public bool? Clrpm2 { get; set; }

    public string? ARem { get; set; }

    public string? CRem { get; set; }

    public string? ConId { get; set; }
    public string? InflammationOfGingiva { get; set; }
    public string? PresenceOfDebris { get; set; }
    public string? PresenceOfCalculus { get; set; }
    public string? PresenceOfStains { get; set; }
    public string? UnderOrthodonticTreatment { get; set; }
    public string? OtherClinicalFindings { get; set; }
}
