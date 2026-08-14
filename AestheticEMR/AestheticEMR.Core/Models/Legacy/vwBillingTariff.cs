using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingTariff
{
    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string? MapTo { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }
}
