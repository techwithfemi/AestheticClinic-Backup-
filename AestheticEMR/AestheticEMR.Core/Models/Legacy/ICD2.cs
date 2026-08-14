using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ICD2")]
public partial class ICD2
{
    [StringLength(50)]
    [Unicode(false)]
    public string? Code { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? DescShort { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? DescLong { get; set; }
}
