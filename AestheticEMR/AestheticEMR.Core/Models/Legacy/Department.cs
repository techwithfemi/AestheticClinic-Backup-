using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Department
{
    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string DeptName { get; set; } = null!;

    [StringLength(100)]
    public string? DeptAddress { get; set; }

    [StringLength(10)]
    public string? Location { get; set; }
}
