using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DepartmentsOLD")]
public partial class DepartmentsOLD
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string? DeptID { get; set; }

    [StringLength(100)]
    public string? DeptName { get; set; }

    [StringLength(200)]
    public string? DeptAddress { get; set; }

    [StringLength(50)]
    public string? COYid { get; set; }
}
