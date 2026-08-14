using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("Users_Table")]
public partial class Users_Table
{
    [Key]
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

    [StringLength(1000)]
    [Unicode(false)]
    public string? SaltedPass { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdated { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }
}
