using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HormonalAssayOLD")]
public partial class HormonalAssayOLD
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? TSH { get; set; }

    [StringLength(50)]
    public string? TSH_NEONATAL { get; set; }

    [StringLength(50)]
    public string? THYROXINE { get; set; }

    [StringLength(50)]
    public string? TRIIODO { get; set; }

    [StringLength(50)]
    public string? T4_FREE { get; set; }

    [StringLength(50)]
    public string? T3_FREE { get; set; }

    [StringLength(50)]
    public string? THYROID_A_T3_FREE { get; set; }

    [StringLength(50)]
    public string? THYROID_A_T4_FREE { get; set; }

    [StringLength(50)]
    public string? THYROID_A_TSH { get; set; }

    [StringLength(50)]
    public string? THYROID_B_T3 { get; set; }

    [StringLength(50)]
    public string? THYROID_B_T4 { get; set; }

    [StringLength(50)]
    public string? THYROID_B_TSH { get; set; }

    [StringLength(50)]
    public string? THYROID_C_T3 { get; set; }

    [StringLength(50)]
    public string? THYROID_C_T4 { get; set; }

    [StringLength(50)]
    public string? THYROID_C_TSH { get; set; }

    [StringLength(50)]
    public string? THYROID_C_T_ANTIBOD { get; set; }

    [StringLength(50)]
    public string? THYROID_ANTIBOD_MACROM { get; set; }

    [StringLength(50)]
    public string? TSH_RECEPTOR { get; set; }

    [StringLength(50)]
    public string? PTH { get; set; }

    [StringLength(50)]
    public string? B_HCG { get; set; }

    [StringLength(50)]
    public string? PROLACTIN { get; set; }

    [StringLength(50)]
    public string? HFSH { get; set; }

    [StringLength(50)]
    public string? HLH { get; set; }

    [StringLength(50)]
    public string? OESTRADIOL { get; set; }

    [StringLength(50)]
    public string? PROGEST { get; set; }

    [StringLength(50)]
    public string? DHEA_S { get; set; }

    [StringLength(50)]
    public string? TESTOSTE { get; set; }

    [StringLength(50)]
    public string? MENOUP_SCREEN_LH { get; set; }

    [StringLength(50)]
    public string? MENOUP_SCREEN_FSH { get; set; }

    [StringLength(50)]
    public string? MENOUP_SCREEN_ESTROG { get; set; }

    [StringLength(50)]
    public string? MENSTR_DIORD_VIRI_LH { get; set; }

    [StringLength(50)]
    public string? MENSTR_DIORD_VIRI_FSH { get; set; }

    [StringLength(50)]
    public string? MENSTR_DIORD_VIRI_PROL { get; set; }

    [StringLength(50)]
    public string? MENSTR_DIORD_VIRI_TESTO { get; set; }

    [StringLength(50)]
    public string? MENSTR_DIORD_VIRI_DHEA_S { get; set; }
}
