using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAdmissionForCoy
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(3000)]
    public string prescription { get; set; } = null!;

    [StringLength(3000)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    public string? diffDiagnosis { get; set; }

    public string? investigate { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(150)]
    public string coyNAme { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischTime { get; set; }

    public int? NoOfDays { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }
}
