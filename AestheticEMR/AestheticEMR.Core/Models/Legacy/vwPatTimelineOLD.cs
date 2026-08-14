using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPatTimelineOLD
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Attendance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Vitals { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Consulting { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Pharmacy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(3000)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    public string? prescription { get; set; }

    [StringLength(3000)]
    public string? investigate { get; set; }

    [StringLength(3000)]
    public string? services { get; set; }

    public double? AmountGen { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Inv { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Bill { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ApptClinic { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? ApptRemarks { get; set; }
}
