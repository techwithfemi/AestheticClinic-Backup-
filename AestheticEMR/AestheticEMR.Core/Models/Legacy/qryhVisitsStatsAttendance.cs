using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVisitsStatsAttendance
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    public int recID { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

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
    public string? Area { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LatestBillNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(101)]
    public string? UserName { get; set; }

    [StringLength(22)]
    [Unicode(false)]
    public string? BioID { get; set; }

    [StringLength(50)]
    public string? clientCatID2 { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime date1 { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HmoRef { get; set; }

    public int? Age { get; set; }

    public int? AgeInMths { get; set; }

    public int? AgeInDays { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Treatment { get; set; }

    [StringLength(4000)]
    public string? LabResult { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string? DateAndTime { get; set; }
}
