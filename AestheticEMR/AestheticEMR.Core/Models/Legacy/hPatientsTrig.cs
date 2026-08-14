using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsTrig")]
public partial class hPatientsTrig
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string pCatID { get; set; } = null!;

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegDate { get; set; }

    [StringLength(50)]
    public string FileDuration { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(100)]
    public string homeAddress { get; set; } = null!;

    [StringLength(100)]
    public string? officeAddress { get; set; }

    [StringLength(50)]
    public string? pPhoneNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(3)]
    public string? Ref { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? branch { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [StringLength(50)]
    public string? relationToStaff { get; set; }

    [StringLength(50)]
    public string? introducedby { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(50)]
    public string? CardType { get; set; }

    [StringLength(200)]
    public string? pMembers { get; set; }

    [StringLength(50)]
    public string? bloodGroup { get; set; }

    [StringLength(50)]
    public string? genotype { get; set; }

    [StringLength(50)]
    public string? occupation { get; set; }

    [StringLength(50)]
    public string? religion { get; set; }

    [StringLength(50)]
    public string? nextOfKin { get; set; }

    [StringLength(50)]
    public string? relationToKin { get; set; }

    [StringLength(150)]
    public string? kinAddress { get; set; }

    public bool? expired { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Maturity { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(4000)]
    public string? DrgRxn { get; set; }

    [StringLength(50)]
    public string? CoyClass { get; set; }

    [StringLength(50)]
    public string? NOKPhone { get; set; }

    [StringLength(50)]
    public string? Title { get; set; }
}
