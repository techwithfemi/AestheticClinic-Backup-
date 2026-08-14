using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hScanPix")]
public partial class hScanPix
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ScanDesc { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PixCode { get; set; } = null!;
}
