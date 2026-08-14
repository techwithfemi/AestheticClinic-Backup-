using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("YesNo")]
public partial class YesNo
{
    [Column("YesNo")]
    [StringLength(3)]
    public string? YesNo1 { get; set; }
}
