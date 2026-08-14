using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillHeading
{
    public long SNo { get; set; }

    [StringLength(350)]
    public string? HeadName { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }
}
