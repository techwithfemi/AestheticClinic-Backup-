using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugCategory
{
    public string DrgCatName { get; set; } = null!;

    public string? CatRemarks { get; set; }

    public string? DrgCatGroup { get; set; }

    public string? DeptBillCenter { get; set; }

    public string? DrgCatCode { get; set; }

    public string? RptHead { get; set; }

    public string? DeptId { get; set; }
}
