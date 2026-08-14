using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TestUnique")]
public partial class TestUnique
{
    public Guid? SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SName { get; set; }
}
