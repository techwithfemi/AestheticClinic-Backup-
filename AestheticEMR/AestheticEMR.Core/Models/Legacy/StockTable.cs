using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class StockTable
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string tblName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ColName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
