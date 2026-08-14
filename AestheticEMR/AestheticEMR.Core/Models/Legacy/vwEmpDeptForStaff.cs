using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwEmpDeptForStaff
{
    [StringLength(100)]
    public string empID { get; set; } = null!;

    [StringLength(50)]
    public string DeptName { get; set; } = null!;

    [StringLength(200)]
    public string Designation { get; set; } = null!;
}
