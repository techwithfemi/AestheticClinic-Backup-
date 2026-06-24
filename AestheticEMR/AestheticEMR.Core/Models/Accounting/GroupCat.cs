using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class GroupCat
{
    public long SNo { get; set; }

    public string CatID { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public long CatMin { get; set; }

    public long CatMax { get; set; }

    public string? Remarks { get; set; }

    public string CatMasterID { get; set; } = null!;

    public bool? HiddenCat { get; set; }
}
