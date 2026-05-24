using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNewRegForTodayAnc
{
    public DateTime RegDate { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string? CardType { get; set; }

    public string PCatId { get; set; } = null!;
}
