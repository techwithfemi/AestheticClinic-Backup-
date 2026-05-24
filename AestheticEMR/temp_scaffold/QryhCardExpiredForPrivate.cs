using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhCardExpiredForPrivate
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public DateTime RegDate { get; set; }

    public string FileDuration { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public bool? Expired { get; set; }

    public string? Numval { get; set; }

    public string? PCatId { get; set; }
}
