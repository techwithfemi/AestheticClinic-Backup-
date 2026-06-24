using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGroupItem
{
    public long SNo { get; set; }

    public string GroupID { get; set; } = null!;

    public string CatID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public bool GroupHidden { get; set; }

    public double GroupDBAmt { get; set; }

    public double GroupCrAmt { get; set; }

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public string? Remarks { get; set; }

    public string CatMasterID { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public long CatMasterMin { get; set; }

    public long CatMasterMax { get; set; }

    public string Expr1 { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public long CatMin { get; set; }

    public long CatMax { get; set; }

    public bool? HiddenCat { get; set; }

    public bool? HiddenGp { get; set; }

    public string Editable { get; set; } = null!;

    public short? RptSerial { get; set; }

    public string? RptType { get; set; }

    public string? RptLevel { get; set; }

    public string? RptTitle { get; set; }

    public string? CanDepr { get; set; }
}
