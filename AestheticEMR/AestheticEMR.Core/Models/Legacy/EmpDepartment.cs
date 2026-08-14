using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class EmpDepartment
{
    [Key]
    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string DeptName { get; set; } = null!;

    [StringLength(200)]
    public string? DeptAddress { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }
}
