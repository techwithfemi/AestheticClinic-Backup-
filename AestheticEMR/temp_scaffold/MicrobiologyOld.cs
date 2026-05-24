using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class MicrobiologyOld
{
    public long Id { get; set; }

    public string Labno { get; set; } = null!;

    public string? STyphiO { get; set; }

    public string? STyphiH { get; set; }

    public string? SParatyphAO { get; set; }

    public string? SParatyphAH { get; set; }

    public string? SParatyphBO { get; set; }

    public string? SParatyphBH { get; set; }

    public string? SParatyphCO { get; set; }

    public string? SParatyphCH { get; set; }

    public string? SkinScrap { get; set; }

    public string? Skinsnip { get; set; }

    public string? HeafMantoux { get; set; }

    public string? Afb { get; set; }

    public string? Cytology { get; set; }

    public string? STimeProduced { get; set; }

    public string? STimeRecvd { get; set; }

    public string? Color { get; set; }

    public string? Vol { get; set; }

    public string? Ph { get; set; }

    public string? Consist { get; set; }

    public string? Liquefa { get; set; }

    public string? Motility { get; set; }

    public string? SActive { get; set; }

    public string? Sluggish { get; set; }

    public string? SDead { get; set; }

    public string? TSpermCount { get; set; }

    public string? MorphNorm { get; set; }

    public string? MorphAbnorm { get; set; }

    public string? Puscells { get; set; }

    public string? Rbc { get; set; }

    public string? EpithCells { get; set; }

    public string? Others { get; set; }

    public string? Culture { get; set; }
}
