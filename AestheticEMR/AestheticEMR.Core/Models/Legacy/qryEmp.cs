using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryEmp
{
    [StringLength(100)]
    public string empID { get; set; } = null!;

    [StringLength(201)]
    public string empFullname { get; set; } = null!;

    [StringLength(100)]
    public string desID { get; set; } = null!;

    [StringLength(200)]
    public string desName { get; set; } = null!;

    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string DeptName { get; set; } = null!;
}
