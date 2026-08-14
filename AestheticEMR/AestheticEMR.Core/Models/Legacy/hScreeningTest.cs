using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hScreeningTest")]
public partial class hScreeningTest
{
    public long SNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemTest { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
