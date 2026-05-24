using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HaemAndSerologyOld
{
    public long Id { get; set; }

    public string Labno { get; set; } = null!;

    public string? Pcv { get; set; }

    public string? Hb { get; set; }

    public string? Mchc { get; set; }

    public string? Wbc { get; set; }

    public string? Eosinophils { get; set; }

    public string? Platelets { get; set; }

    public string? Rbc { get; set; }

    public string? Retics { get; set; }

    public string? Esr { get; set; }

    public string? Microfilaria { get; set; }

    public string? Malaria { get; set; }

    public string? Anicotysis { get; set; }

    public string? Piokil { get; set; }

    public string? Polychrom { get; set; }

    public string? Macrocyto { get; set; }

    public string? Hypochrom { get; set; }

    public string? Sickle { get; set; }

    public string? Targetcells { get; set; }

    public string? NucleatedRbc { get; set; }

    public string? Spherocyto { get; set; }

    public string? Neutophils { get; set; }

    public string? Lymphocytes { get; set; }

    public string? Monocytes { get; set; }

    public string? EosinphilsDiff { get; set; }

    public string? Basophils { get; set; }

    public string? Bloodgrp { get; set; }

    public string? Slickingtest { get; set; }

    public string? Genotype { get; set; }

    public string? ProthromeTime { get; set; }

    public string? Contr { get; set; }

    public string? BleedTime { get; set; }

    public string? ClotTime { get; set; }
}
