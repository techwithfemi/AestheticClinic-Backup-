using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class StockDept
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptName { get; set; }
}
