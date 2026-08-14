using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetailsGroupedPrivate
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string DrgName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    public double? SubTotal { get; set; }

    public double Price { get; set; }

    public double Qty { get; set; }
}
