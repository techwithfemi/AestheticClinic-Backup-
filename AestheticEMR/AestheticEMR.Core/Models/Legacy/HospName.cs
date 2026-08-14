using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HospName")]
public partial class HospName
{
    [StringLength(50)]
    public string? hospID { get; set; }

    [StringLength(250)]
    public string HName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Location { get; set; }
}
