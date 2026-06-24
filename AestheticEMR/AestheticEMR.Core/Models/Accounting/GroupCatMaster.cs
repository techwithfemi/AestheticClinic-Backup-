using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class GroupCatMaster
{
    public long SNo { get; set; }

    public string CatMasterID { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public long CatMasterMin { get; set; }

    public long CatMasterMax { get; set; }

    public string? Remarks { get; set; }

    public bool? HiddenMaster { get; set; }

    public string? BalStatus { get; set; }
}
