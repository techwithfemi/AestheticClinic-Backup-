using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hECG")]
public partial class hECG
{
    public long ID { get; set; }

    [StringLength(250)]
    public string PNO { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(50)]
    public string? EMPID { get; set; }

    public bool? ATTENDEDTO { get; set; }

    [StringLength(50)]
    public string? PulseRate { get; set; }

    [StringLength(50)]
    public string? PulseRhythm { get; set; }

    [StringLength(50)]
    public string? PulseAxis { get; set; }

    [StringLength(50)]
    public string? PulseBPM { get; set; }

    [StringLength(50)]
    public string? PulseWt { get; set; }

    [StringLength(50)]
    public string? PulseHt { get; set; }

    [StringLength(50)]
    public string? WaveP { get; set; }

    [StringLength(50)]
    public string? WaveT { get; set; }

    [StringLength(50)]
    public string? WaveST { get; set; }

    [StringLength(50)]
    public string? IntPP { get; set; }

    [StringLength(50)]
    public string? IntQRS { get; set; }

    [StringLength(50)]
    public string? IntQT { get; set; }

    [StringLength(50)]
    public string? CharmSV1 { get; set; }

    [StringLength(50)]
    public string? ChamS27 { get; set; }

    [StringLength(4000)]
    public string? Conclusion { get; set; }

    public long? ConID { get; set; }

    [StringLength(50)]
    public string? CLASS { get; set; }
}
