using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("UserInfo")]
public partial class UserInfo
{
    [Key]
    [StringLength(50)]
    public string UserID { get; set; } = null!;

    [StringLength(50)]
    public string? Username { get; set; }

    [StringLength(50)]
    public string? Firstname { get; set; }

    [StringLength(50)]
    public string? Lastname { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? Designation { get; set; }
}
