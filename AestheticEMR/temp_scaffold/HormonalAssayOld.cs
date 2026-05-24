using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HormonalAssayOld
{
    public long Id { get; set; }

    public string Labno { get; set; } = null!;

    public string? Tsh { get; set; }

    public string? TshNeonatal { get; set; }

    public string? Thyroxine { get; set; }

    public string? Triiodo { get; set; }

    public string? T4Free { get; set; }

    public string? T3Free { get; set; }

    public string? ThyroidAT3Free { get; set; }

    public string? ThyroidAT4Free { get; set; }

    public string? ThyroidATsh { get; set; }

    public string? ThyroidBT3 { get; set; }

    public string? ThyroidBT4 { get; set; }

    public string? ThyroidBTsh { get; set; }

    public string? ThyroidCT3 { get; set; }

    public string? ThyroidCT4 { get; set; }

    public string? ThyroidCTsh { get; set; }

    public string? ThyroidCTAntibod { get; set; }

    public string? ThyroidAntibodMacrom { get; set; }

    public string? TshReceptor { get; set; }

    public string? Pth { get; set; }

    public string? BHcg { get; set; }

    public string? Prolactin { get; set; }

    public string? Hfsh { get; set; }

    public string? Hlh { get; set; }

    public string? Oestradiol { get; set; }

    public string? Progest { get; set; }

    public string? DheaS { get; set; }

    public string? Testoste { get; set; }

    public string? MenoupScreenLh { get; set; }

    public string? MenoupScreenFsh { get; set; }

    public string? MenoupScreenEstrog { get; set; }

    public string? MenstrDiordViriLh { get; set; }

    public string? MenstrDiordViriFsh { get; set; }

    public string? MenstrDiordViriProl { get; set; }

    public string? MenstrDiordViriTesto { get; set; }

    public string? MenstrDiordViriDheaS { get; set; }
}
