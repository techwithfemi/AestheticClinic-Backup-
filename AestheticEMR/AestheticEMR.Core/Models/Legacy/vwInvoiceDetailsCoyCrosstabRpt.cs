using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoyCrosstabRpt
{
    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    public int? NoOfDays { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? CONSULTATION { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? DRUG { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? INJECTION { get; set; }

    [Column("INFUSION/TRANSFUSION", TypeName = "decimal(38, 2)")]
    public decimal? INFUSION_TRANSFUSION { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? LAB { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? SCAN { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? ECG { get; set; }

    [Column("X-RAY", TypeName = "decimal(38, 2)")]
    public decimal? X_RAY { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? PROCEDURE { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? ADMISSION { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? DENTAL { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? EYE { get; set; }

    [Column("M/SURGERY", TypeName = "decimal(38, 2)")]
    public decimal? M_SURGERY { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? FEEDING { get; set; }
}
