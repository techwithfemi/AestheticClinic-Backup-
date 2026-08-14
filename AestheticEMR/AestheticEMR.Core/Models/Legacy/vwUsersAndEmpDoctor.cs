using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwUsersAndEmpDoctor
{
    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(50)]
    public string? UserName { get; set; }

    [StringLength(101)]
    public string empFullname { get; set; } = null!;

    [StringLength(50)]
    public string Department { get; set; } = null!;

    [StringLength(100)]
    public string Designation { get; set; } = null!;

    [StringLength(50)]
    public string DeptID { get; set; } = null!;

    [StringLength(50)]
    public string desID { get; set; } = null!;

    [StringLength(65)]
    public string? Fullname { get; set; }

    [StringLength(18)]
    public string? Password { get; set; }

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(50)]
    public string? BranchCode { get; set; }
}
