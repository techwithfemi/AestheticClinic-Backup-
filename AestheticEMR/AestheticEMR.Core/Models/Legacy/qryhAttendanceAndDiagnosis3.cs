using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAttendanceAndDiagnosis3
{
    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(3000)]
    public string? DiagConsult { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(3000)]
    public string? symptoms { get; set; }

    [StringLength(4000)]
    public string prescription { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? nextApptDate { get; set; }

    [StringLength(3000)]
    public string? complaints { get; set; }

    [StringLength(3000)]
    public string? investigate { get; set; }

    [StringLength(101)]
    public string? treatedBy { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(150)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? oldpNo { get; set; }

    public int? Age { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }
}
