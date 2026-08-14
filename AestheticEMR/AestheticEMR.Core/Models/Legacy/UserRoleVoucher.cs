using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UserRoleVoucher")]
public partial class UserRoleVoucher
{
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(2)]
    public string SetID { get; set; } = null!;
}
