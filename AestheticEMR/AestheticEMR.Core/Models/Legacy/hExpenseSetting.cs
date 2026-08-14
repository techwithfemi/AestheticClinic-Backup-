using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hExpenseSetting")]
public partial class hExpenseSetting
{
    public long SNo { get; set; }

    [StringLength(2)]
    public string SetID { get; set; } = null!;

    [StringLength(50)]
    public string SetName { get; set; } = null!;
}
