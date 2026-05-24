using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHistPathologyPublic
{
    public string LabNo { get; set; } = null!;

    public string? PathNo { get; set; }

    public string? Clinician { get; set; }

    public string? EtnicGroup { get; set; }

    public string? Ward { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? Test { get; set; }

    public string? Maternal { get; set; }

    public DateTime DtDate { get; set; }

    public string Report { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public string DocName { get; set; } = null!;

    public string HospName { get; set; } = null!;

    public string Expr1 { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string ResultMaster { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string? EmpName { get; set; }

    public string? Description { get; set; }

    public string? Result { get; set; }

    public string? Desc2 { get; set; }

    public string? Sample { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public string? ConId { get; set; }

    public string? LabNum { get; set; }
}
