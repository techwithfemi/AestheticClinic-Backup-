using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class WeekDay
{
    public int SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string WkDay { get; set; } = null!;
}
