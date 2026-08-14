using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Roles1")]
public partial class Roles1
{
    [StringLength(50)]
    public string RoleID { get; set; } = null!;

    [StringLength(18)]
    public string? LoginRole { get; set; }
}
