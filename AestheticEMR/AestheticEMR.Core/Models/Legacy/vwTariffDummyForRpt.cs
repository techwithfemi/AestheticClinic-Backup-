using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTariffDummyForRpt
{
    [StringLength(1)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    public int Price { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Capitated { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Company { get; set; } = null!;
}
