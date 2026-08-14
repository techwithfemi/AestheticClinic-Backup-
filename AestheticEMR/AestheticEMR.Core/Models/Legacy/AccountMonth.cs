using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("AccountMonth")]
public partial class AccountMonth
{
    [Column(TypeName = "datetime")]
    public DateTime? AcctMonth { get; set; }

    public int MonthCounter { get; set; }

    [StringLength(50)]
    public string? PeriodYr { get; set; }
}
