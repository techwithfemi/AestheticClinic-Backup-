using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class drugNHISListNotInDrug
{
    [StringLength(255)]
    public string DrgName { get; set; } = null!;

    [StringLength(255)]
    public string? PharmCat { get; set; }

    [StringLength(255)]
    public string? DrgCatName { get; set; }

    public double Price { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    public long SNO { get; set; }

    [StringLength(255)]
    public string? CoyName { get; set; }
}
