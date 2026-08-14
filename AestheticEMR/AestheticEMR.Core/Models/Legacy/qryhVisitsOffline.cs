using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVisitsOffline
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    public int recID { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? branch { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(50)]
    public string Surname { get; set; } = null!;
}
