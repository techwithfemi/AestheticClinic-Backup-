using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPatListForNurse
{
    public string Pno { get; set; } = null!;

    public string PSurName { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ClientCat { get; set; }

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public string? Remarks { get; set; }

    public string ClinicType { get; set; } = null!;

    public int? Age { get; set; }

    public string Company { get; set; } = null!;

    public string? CoyName { get; set; }

    public int RecId { get; set; }

    public DateTime? Htime { get; set; }

    public bool? Suppres { get; set; }

    public string? Title { get; set; }

    public string RetainName { get; set; } = null!;

    public string? OldPno { get; set; }

    public string? Sex { get; set; }

    public DateTime? Dob { get; set; }

    public string? FullName { get; set; }

    public string? Expr1 { get; set; }

    public DateTime? RetainDate { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? DocAssigned { get; set; }

    public string? Doctor { get; set; }
}
