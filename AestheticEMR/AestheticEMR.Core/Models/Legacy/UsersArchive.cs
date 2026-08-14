using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UsersArchive")]
public partial class UsersArchive
{
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(65)]
    public string? Fullname { get; set; }

    [StringLength(18)]
    public string? Password { get; set; }

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(50)]
    public string? BranchCode { get; set; }

    [StringLength(50)]
    public string? AppType { get; set; }

    [StringLength(50)]
    public string? Clinic { get; set; }

    [StringLength(2)]
    public string? UserLevel { get; set; }

    public long SNo { get; set; }
}
