using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class VwAccountsInfo
{
    public long Sno { get; set; }

    public string AccountNo { get; set; } = null!;

    public string AccountId { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyId { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string? AccountDesc { get; set; }

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountCat { get; set; }

    public string? Remarks { get; set; }

    public bool? HiddenGp { get; set; }

    public string CatName { get; set; } = null!;

    public string CatId { get; set; } = null!;

    public bool? IsDummy { get; set; }

    public string? RptType { get; set; }

    public bool? Hidden { get; set; }

    public string? RptLevel { get; set; }

    public bool? Suppres { get; set; }

    public string? RptType2 { get; set; }

    public bool GroupHidden { get; set; }

    public string CatMasterId { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public short? RptSerial { get; set; }

    public string? RptTitle { get; set; }
}
