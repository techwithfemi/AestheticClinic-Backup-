using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HaemAndSerologyOLD")]
public partial class HaemAndSerologyOLD
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? PCV { get; set; }

    [StringLength(50)]
    public string? HB { get; set; }

    [StringLength(50)]
    public string? MCHC { get; set; }

    [StringLength(50)]
    public string? WBC { get; set; }

    [StringLength(50)]
    public string? EOSINOPHILS { get; set; }

    [StringLength(50)]
    public string? PLATELETS { get; set; }

    [StringLength(50)]
    public string? RBC { get; set; }

    [StringLength(50)]
    public string? RETICS { get; set; }

    [StringLength(50)]
    public string? ESR { get; set; }

    [StringLength(50)]
    public string? MICROFILARIA { get; set; }

    [StringLength(50)]
    public string? MALARIA { get; set; }

    [StringLength(50)]
    public string? ANICOTYSIS { get; set; }

    [StringLength(50)]
    public string? PIOKIL { get; set; }

    [StringLength(50)]
    public string? POLYCHROM { get; set; }

    [StringLength(50)]
    public string? MACROCYTO { get; set; }

    [StringLength(50)]
    public string? HYPOCHROM { get; set; }

    [StringLength(50)]
    public string? SICKLE { get; set; }

    [StringLength(50)]
    public string? TARGETCELLS { get; set; }

    [StringLength(50)]
    public string? NUCLEATED_RBC { get; set; }

    [StringLength(50)]
    public string? SPHEROCYTO { get; set; }

    [StringLength(50)]
    public string? NEUTOPHILS { get; set; }

    [StringLength(50)]
    public string? LYMPHOCYTES { get; set; }

    [StringLength(50)]
    public string? MONOCYTES { get; set; }

    [StringLength(50)]
    public string? EOSINPHILS_DIFF { get; set; }

    [StringLength(50)]
    public string? BASOPHILS { get; set; }

    [StringLength(50)]
    public string? BLOODGRP { get; set; }

    [StringLength(50)]
    public string? SLICKINGTEST { get; set; }

    [StringLength(50)]
    public string? GENOTYPE { get; set; }

    [StringLength(50)]
    public string? PROTHROME_TIME { get; set; }

    [StringLength(50)]
    public string? CONTR { get; set; }

    [StringLength(50)]
    public string? BLEED_TIME { get; set; }

    [StringLength(50)]
    public string? CLOT_TIME { get; set; }
}
