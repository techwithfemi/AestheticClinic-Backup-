using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UserRoleClinic")]
public partial class UserRoleClinic
{
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    public string clinicID { get; set; } = null!;
}
