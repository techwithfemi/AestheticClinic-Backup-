using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseCat
{
    public string CatCode { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string? AcctId { get; set; }

    public string CatType { get; set; } = null!;

    public string? Description { get; set; }
}
