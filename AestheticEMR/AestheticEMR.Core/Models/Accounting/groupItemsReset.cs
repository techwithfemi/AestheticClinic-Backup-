using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class groupItemsReset
{
    public long SNo { get; set; }

    public string GroupID { get; set; } = null!;

    public string CatID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? RptTitle { get; set; }

    public bool? HiddenGp { get; set; }

    public bool? Suppres { get; set; }

    public bool GroupHidden { get; set; }

    public short? RptSerial { get; set; }

    public string? RptType { get; set; }

    public string? RptLevel { get; set; }

    public string? Remarks { get; set; }

    public double GroupCrAmt { get; set; }

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public double GroupDBAmt { get; set; }

    public bool? Editable { get; set; }
}
