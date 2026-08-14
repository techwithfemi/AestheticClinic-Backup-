using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hdisease
{
    public long ID { get; set; }

    [Unicode(false)]
    public string? disease { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Code { get; set; }
}
