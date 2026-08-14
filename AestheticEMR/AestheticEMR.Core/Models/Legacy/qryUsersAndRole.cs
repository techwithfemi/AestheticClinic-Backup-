using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUsersAndRole
{
    [StringLength(50)]
    public string? RoleID { get; set; }

    [StringLength(18)]
    public string? LoginRole { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(18)]
    public string? Password { get; set; }

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(50)]
    public string? BranchCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    [StringLength(50)]
    public string? AppType { get; set; }

    [StringLength(50)]
    public string? Clinic { get; set; }

    [StringLength(2)]
    public string? UserLevel { get; set; }

    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(50)]
    public string desID { get; set; } = null!;

    [StringLength(100)]
    public string Designation { get; set; } = null!;
}
