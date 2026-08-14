using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("MicrobiologyOLD")]
public partial class MicrobiologyOLD
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? S_TYPHI_O { get; set; }

    [StringLength(50)]
    public string? S_TYPHI_H { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_A_O { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_A_H { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_B_O { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_B_H { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_C_O { get; set; }

    [StringLength(50)]
    public string? S_PARATYPH_C_H { get; set; }

    [StringLength(50)]
    public string? SKIN_SCRAP { get; set; }

    [StringLength(50)]
    public string? SKINSNIP { get; set; }

    [StringLength(50)]
    public string? HEAF_MANTOUX { get; set; }

    [StringLength(50)]
    public string? AFB { get; set; }

    [StringLength(50)]
    public string? CYTOLOGY { get; set; }

    [StringLength(50)]
    public string? S_TIME_PRODUCED { get; set; }

    [StringLength(50)]
    public string? S_TIME_RECVD { get; set; }

    [StringLength(50)]
    public string? COLOR { get; set; }

    [StringLength(50)]
    public string? VOL { get; set; }

    [StringLength(50)]
    public string? PH { get; set; }

    [StringLength(50)]
    public string? CONSIST { get; set; }

    [StringLength(50)]
    public string? LIQUEFA { get; set; }

    [StringLength(50)]
    public string? MOTILITY { get; set; }

    [StringLength(50)]
    public string? S_ACTIVE { get; set; }

    [StringLength(50)]
    public string? SLUGGISH { get; set; }

    [StringLength(50)]
    public string? S_DEAD { get; set; }

    [StringLength(50)]
    public string? T_SPERM_COUNT { get; set; }

    [StringLength(50)]
    public string? MORPH_NORM { get; set; }

    [StringLength(50)]
    public string? MORPH_ABNORM { get; set; }

    [StringLength(50)]
    public string? PUSCELLS { get; set; }

    [StringLength(50)]
    public string? RBC { get; set; }

    [StringLength(50)]
    public string? EPITH_CELLS { get; set; }

    [StringLength(50)]
    public string? OTHERS { get; set; }

    [StringLength(50)]
    public string? CULTURE { get; set; }
}
