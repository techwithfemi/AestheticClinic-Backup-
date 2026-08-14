using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatients251019")]
public partial class hPatients251019
{
    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string pSurName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(1100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Occupation { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? OfficeAddress { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Religion { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? NextofKin { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? kinAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? relationToKin { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MStatus { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? BloodGroup { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Genotype { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyNAme { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clientCatID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RegDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FileDuration { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Ref { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? empNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? branch { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? status { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? relationToStaff { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? introducedby { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CardType { get; set; }

    [StringLength(200)]
    public string? pMembers { get; set; }

    public bool? expired { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Maturity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Debt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Color { get; set; }

    [StringLength(4000)]
    public string? DrgRxn { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CoyClass { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? NOKPhone { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HmoRef { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Principal { get; set; }

    [StringLength(4000)]
    public string? PastMedHist { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Area { get; set; }

    [Column(TypeName = "image")]
    public byte[]? PatPix { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LatestBillNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAttndDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LastConsultID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UserName { get; set; }

    public bool? isEnrol { get; set; }

    [StringLength(22)]
    [Unicode(false)]
    public string? BioID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }

    [StringLength(4000)]
    [Unicode(false)]
    public string? ANCInfo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo2 { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? newReg { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? smsNextDOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? smsCat { get; set; }

    public bool? isRev { get; set; }

    public long SNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? LastClinicVisited { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? LastPurpose { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? LastDoctorSeen { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LasConDate { get; set; }
}
