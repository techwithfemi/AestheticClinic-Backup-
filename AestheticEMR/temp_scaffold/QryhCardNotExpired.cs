using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhCardNotExpired
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public DateTime RegDate { get; set; }

    public string FileDuration { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string HomeAddress { get; set; } = null!;

    public string? OfficeAddress { get; set; }

    public string? PPhoneNo { get; set; }

    public DateTime Dob { get; set; }

    public string? Email { get; set; }

    public string Sex { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string? CoyType { get; set; }

    public string? CoyName { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? RelationToStaff { get; set; }

    public string? Introducedby { get; set; }

    public string? PolicyType { get; set; }

    public string? CardType { get; set; }

    public string? PMembers { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Occupation { get; set; }

    public string? Religion { get; set; }

    public string NextOfKin { get; set; } = null!;

    public string? RelationToKin { get; set; }

    public string? KinAddress { get; set; }

    public bool Expired { get; set; }
}
