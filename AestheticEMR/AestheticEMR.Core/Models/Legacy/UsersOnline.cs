using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("UsersOnline")]
public partial class UsersOnline
{
    [Key]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? EmpID { get; set; }

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

    [StringLength(150)]
    public string? Email { get; set; }
}
