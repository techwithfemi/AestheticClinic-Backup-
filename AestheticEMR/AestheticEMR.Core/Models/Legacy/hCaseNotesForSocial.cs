using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hCaseNotesForSocial")]
public partial class hCaseNotesForSocial
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime nDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? nTime { get; set; }

    [StringLength(2500)]
    public string notes { get; set; } = null!;

    [StringLength(2500)]
    public string? ActionPlan { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(250)]
    public string MedicalDiagnosis { get; set; } = null!;

    [StringLength(500)]
    public string? SocialDiagnosis { get; set; }
}
