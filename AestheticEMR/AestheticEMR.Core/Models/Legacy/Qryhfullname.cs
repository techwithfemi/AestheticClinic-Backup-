using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Qryhfullname
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? PolicyType { get; set; }

    public string? CoyType { get; set; }

    public string? PCatId { get; set; }

    public string? ClientCatId { get; set; }

    public string? HomeAddress { get; set; }

    public string? OfficeAddress { get; set; }

    public DateTime? Dob { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Sex { get; set; }

    public string? Occupation { get; set; }

    public int? Age { get; set; }

    public string? Ref { get; set; }

    public string? Status { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? EmpNo { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public double? Debt { get; set; }

    public string? Branch { get; set; }

    public string? Company { get; set; }

    public string? Email { get; set; }

    public string? Maturity { get; set; }

    public string? DrgRxn { get; set; }

    public string? Title { get; set; }
}
