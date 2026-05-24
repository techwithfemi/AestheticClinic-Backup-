using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhHmolist
{
    public string? EmpNo { get; set; }

    public string? RelationToStaff { get; set; }

    public string Fullname { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string? PolicyType { get; set; }

    public string CoyName { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public string FileDuration { get; set; } = null!;
}
