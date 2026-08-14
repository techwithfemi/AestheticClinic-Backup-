using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwRole
{
    [StringLength(50)]
    public string RoleID { get; set; } = null!;

    [StringLength(18)]
    public string? LoginRole { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Enabled { get; set; }
}
