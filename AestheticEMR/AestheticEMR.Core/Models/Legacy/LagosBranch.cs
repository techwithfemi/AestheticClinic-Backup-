using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class LagosBranch
{
    [Column(TypeName = "decimal(18, 0)")]
    public decimal SerialNo { get; set; }

    [StringLength(20)]
    public string BranchCode { get; set; } = null!;

    [StringLength(25)]
    public string? BranchName { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }
}
