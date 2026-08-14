using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UserRolesOnline")]
public partial class UserRolesOnline
{
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    public string RoleID { get; set; } = null!;

    public long sno { get; set; }
}
