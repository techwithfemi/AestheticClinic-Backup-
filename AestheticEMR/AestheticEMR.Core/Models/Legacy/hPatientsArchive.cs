using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsArchive")]
public partial class hPatientsArchive
{
    [StringLength(20)]
    [Unicode(false)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Occupation { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OfficeAddress { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Religion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NextofKin { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? kinAddress { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string? relationToKin { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RegDate { get; set; }

    [StringLength(50)]
    public string? FileDuration { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

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

    [StringLength(3)]
    public string? HmoRef { get; set; }

    public long SNo { get; set; }

    [StringLength(50)]
    public string? MStatus { get; set; }

    [StringLength(4000)]
    public string? PastMedHist { get; set; }

    [StringLength(150)]
    public string? Area { get; set; }

    [Column(TypeName = "image")]
    public byte[]? PatPix { get; set; }

    [StringLength(50)]
    public string? LatestBillNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAttndDate { get; set; }

    [StringLength(50)]
    public string? LastConsultID { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }
}
