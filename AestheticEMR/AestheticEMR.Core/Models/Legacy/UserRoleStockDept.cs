using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UserRoleStockDept")]
public partial class UserRoleStockDept
{
    [StringLength(50)]
    public string Username { get; set; } = null!;

    public long SNoID { get; set; }
}
