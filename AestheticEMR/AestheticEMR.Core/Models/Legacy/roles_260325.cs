using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("roles_260325")]
public partial class roles_260325
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string RoleID { get; set; } = null!;

    [StringLength(18)]
    public string? LoginRole { get; set; }

    public bool? Enabled { get; set; }

    public int? MinVer { get; set; }
}
