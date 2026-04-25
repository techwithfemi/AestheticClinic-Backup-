using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class vwEmpName
{
    public string empID { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public string Dept { get; set; } = null!;

    public string Designation { get; set; } = null!;
}
