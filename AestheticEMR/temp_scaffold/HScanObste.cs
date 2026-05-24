using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScanObste
{
    public long Id { get; set; }

    public DateTime Invdate { get; set; }

    public string Pno { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Desc2 { get; set; }

    public string Result { get; set; } = null!;

    public string InvResult { get; set; } = null!;

    public string Empid { get; set; } = null!;

    public bool? Attendedto { get; set; }

    public long ConId { get; set; }

    public string? Remarks { get; set; }

    public string? Class { get; set; }

    public string? ImageId { get; set; }

    public string? Crl { get; set; }
}
