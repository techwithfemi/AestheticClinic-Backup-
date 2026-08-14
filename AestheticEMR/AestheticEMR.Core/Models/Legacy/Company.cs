using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Company
{
    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(100)]
    public string CoyName { get; set; } = null!;

    [StringLength(200)]
    public string? CoyLocation { get; set; }
}
