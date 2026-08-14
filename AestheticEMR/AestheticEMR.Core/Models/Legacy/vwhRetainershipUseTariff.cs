using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRetainershipUseTariff
{
    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? UseCoyID { get; set; }

    [StringLength(150)]
    public string? UseName { get; set; }

    [StringLength(50)]
    public string? UseTariff { get; set; }

    public double? PCent { get; set; }

    [StringLength(250)]
    public string? Type { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}
