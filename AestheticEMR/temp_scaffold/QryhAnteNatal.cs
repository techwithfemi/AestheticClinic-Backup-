using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAnteNatal
{
    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public string? ClientCatId { get; set; }

    public string? CardType { get; set; }
}
