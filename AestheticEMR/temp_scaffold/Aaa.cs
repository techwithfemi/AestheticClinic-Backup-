using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Aaa
{
    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ItemCatId { get; set; } = null!;

    public string? CatRemarks { get; set; }

    public string? DrgCatGroup { get; set; }
}
