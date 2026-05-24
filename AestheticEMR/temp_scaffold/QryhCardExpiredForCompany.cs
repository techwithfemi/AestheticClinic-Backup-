using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhCardExpiredForCompany
{
    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public DateTime RegDate { get; set; }

    public string FileDuration { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public bool Expired { get; set; }
}
