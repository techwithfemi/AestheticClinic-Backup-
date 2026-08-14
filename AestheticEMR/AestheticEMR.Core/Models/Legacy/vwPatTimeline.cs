using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPatTimeline
{
    public int SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Attendance { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Vitals { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Consulting { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Pharmacy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? prescription { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? investigate { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? services { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountGen { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Bill { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Inv { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ApptClinic { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? ApptRemarks { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }
}
