using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhFullnamePublic
{
    public string Pno { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string OldpNo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string PolicyType { get; set; } = null!;

    public string CoyType { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string ClientCatId { get; set; } = null!;

    public string HomeAddress { get; set; } = null!;

    public string OfficeAddress { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string? PPhoneNo { get; set; }

    public string Occupation { get; set; } = null!;

    public int? Age { get; set; }

    public string Ref { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string BloodGroup { get; set; } = null!;

    public string Genotype { get; set; } = null!;

    public string EmpNo { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstName { get; set; } = null!;

    public string? Sex { get; set; }

    public double? Debt { get; set; }

    public string Branch { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Maturity { get; set; } = null!;

    public string DrgRxn { get; set; } = null!;

    public string Title { get; set; } = null!;
}
