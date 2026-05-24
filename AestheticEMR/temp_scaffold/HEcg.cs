using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HEcg
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string? Empid { get; set; }

    public bool? Attendedto { get; set; }

    public string? PulseRate { get; set; }

    public string? PulseRhythm { get; set; }

    public string? PulseAxis { get; set; }

    public string? PulseBpm { get; set; }

    public string? PulseWt { get; set; }

    public string? PulseHt { get; set; }

    public string? WaveP { get; set; }

    public string? WaveT { get; set; }

    public string? WaveSt { get; set; }

    public string? IntPp { get; set; }

    public string? IntQrs { get; set; }

    public string? IntQt { get; set; }

    public string? CharmSv1 { get; set; }

    public string? ChamS27 { get; set; }

    public string? Conclusion { get; set; }

    public long? ConId { get; set; }

    public string? Class { get; set; }
}
