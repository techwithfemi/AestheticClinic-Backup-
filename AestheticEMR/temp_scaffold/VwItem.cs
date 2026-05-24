using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwItem
{
    public long Sno { get; set; }

    public string CatCode { get; set; } = null!;

    public string? AcctId { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string Bname { get; set; } = null!;

    public string CatName { get; set; } = null!;
}
