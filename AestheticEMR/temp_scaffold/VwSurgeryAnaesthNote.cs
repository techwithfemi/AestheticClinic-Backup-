using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwSurgeryAnaesthNote
{
    public string ConsultId { get; set; } = null!;

    public long? ConId { get; set; }

    public string? AnaesthNotePre { get; set; }

    public string? AnaesthNotePost { get; set; }
}
