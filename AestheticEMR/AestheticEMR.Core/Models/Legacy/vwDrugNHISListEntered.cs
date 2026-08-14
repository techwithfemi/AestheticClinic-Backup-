using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugNHISListEntered
{
    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(150)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(250)]
    public string Type { get; set; } = null!;
}
