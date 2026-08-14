using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHDialysi
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string SessionNo { get; set; } = null!;

    [StringLength(355)]
    public string FullName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Diagnosis { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ConnectionTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DisconnectionTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? PreDialAssess { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? PostDialAssess { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HIV { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HBsAg { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PCV { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BloodGP { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClottingTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? DialPresc { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MachineType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DialyserType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConcType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Duration { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AccessRoute { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ReqdWtLoss { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TMP { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UFR { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BFR { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Heparin { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? InfusionsDrug { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BloodTransfusion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConnectedBy { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CurrentWt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DryWt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevPostDialWt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? WtGain { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PostDialWt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? WtLoss { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;
}
