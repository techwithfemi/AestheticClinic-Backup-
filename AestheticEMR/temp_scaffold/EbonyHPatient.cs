using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EbonyHPatient
{
    public string Pno { get; set; } = null!;

    public string? OldPno { get; set; }

    public string PSurName { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? Title { get; set; }

    public string? HomeAddress { get; set; }

    public string MiddleName { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string StateOfOrigin { get; set; } = null!;

    public string? Sex { get; set; }

    public DateTime? Dob { get; set; }

    public string? Occupation { get; set; }

    public string? OfficeAddress { get; set; }

    public string? NextofKin { get; set; }

    public string? KinAddress { get; set; }

    public string? RelationToKin { get; set; }

    public string? PPhoneNo { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Email { get; set; }

    public string? Nokphone { get; set; }

    public string Company { get; set; } = null!;

    public string FullName { get; set; } = null!;
}
