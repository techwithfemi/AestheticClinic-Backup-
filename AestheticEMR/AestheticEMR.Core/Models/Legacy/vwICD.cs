using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwICD
{
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string DescShort { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string DescLong { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string DescICD { get; set; } = null!;
}
