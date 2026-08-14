using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UserRoleStock")]
public partial class UserRoleStock
{
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    public string LocID { get; set; } = null!;
}
