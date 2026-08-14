using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAdmissionSocial
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string Ward { get; set; } = null!;

    [StringLength(200)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(400)]
    public string? SocialDiagnosis { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(150)]
    public string? coyNAme { get; set; }

    public bool? isDischarged { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    public int? Age { get; set; }
}
