using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DesignationOLD")]
public partial class DesignationOLD
{
    [StringLength(50)]
    public string? desID { get; set; }

    [StringLength(100)]
    public string? desName { get; set; }

    public long SNO { get; set; }
}
