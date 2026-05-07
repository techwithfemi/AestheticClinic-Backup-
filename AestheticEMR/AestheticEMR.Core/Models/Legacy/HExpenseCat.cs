using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HExpenseCat
{
    public string CatCode { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string? Description { get; set; }

    public string? AcctId { get; set; }

    public string CatType { get; set; } = null!;
}
