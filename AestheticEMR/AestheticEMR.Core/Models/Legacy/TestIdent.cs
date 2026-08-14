using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TestIdent")]
public partial class TestIdent
{
    public long sno { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }
}
