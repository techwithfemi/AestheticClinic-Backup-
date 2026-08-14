using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsExcel")]
public partial class hPatientsExcel
{
    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string OldPNo { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? pSurname { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SEX { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OCCUPATION { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? NextofKin { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OfficeAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Religion { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? kinAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? relationToKin { get; set; }

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

    [StringLength(100)]
    [Unicode(false)]
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

    [StringLength(100)]
    [Unicode(false)]
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

    [StringLength(100)]
    [Unicode(false)]
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

    [StringLength(100)]
    [Unicode(false)]
    public string? PastMedHist { get; set; }

    [StringLength(100)]
    [Unicode(false)]
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

    public bool? isEnrol { get; set; }

    public double? DebtBF { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ANCInfo { get; set; }
}
