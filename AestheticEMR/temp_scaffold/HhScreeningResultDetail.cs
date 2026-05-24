using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HhScreeningResultDetail
{
    public long IndexNo { get; set; }

    public string Labno { get; set; } = null!;

    public string TagName { get; set; } = null!;

    public string LblDesc { get; set; } = null!;

    public string Result { get; set; } = null!;

    public string Range { get; set; } = null!;

    public string Units { get; set; } = null!;

    public long? SnoId { get; set; }
}
