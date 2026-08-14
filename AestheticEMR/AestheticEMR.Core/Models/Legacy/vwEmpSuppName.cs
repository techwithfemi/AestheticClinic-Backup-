using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwEmpSuppName
{
    [StringLength(50)]
    public string? PersNo { get; set; }

    [StringLength(500)]
    public string PersName { get; set; } = null!;

    [StringLength(50)]
    public string Dept { get; set; } = null!;
}
