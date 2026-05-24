using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhAntenatal
{
    public DateTime Date { get; set; }

    public string? FundusHeight { get; set; }

    public string? Presentation { get; set; }

    public string? Relation { get; set; }

    public string? FoetalHeart { get; set; }

    public string? UrineAlbumen { get; set; }

    public string? UrineSugar { get; set; }

    public string? Bp { get; set; }

    public string? Wt { get; set; }

    public string? Pcv { get; set; }

    public string? Oedema { get; set; }

    public string? Remarks { get; set; }

    public string? GestAge { get; set; }

    public string PNo { get; set; } = null!;

    public string? ConsultId { get; set; }
}
