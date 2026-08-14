using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hDelivery")]
public partial class hDelivery
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DelvDate { get; set; }

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Method { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string NameOfDoctor { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string NameOfNurse { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string DelvOutcome { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ApgarScore { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Wt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BirthLength { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? HeadCircumference { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? InjectionTaken { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ImmunizationDone { get; set; }
}
