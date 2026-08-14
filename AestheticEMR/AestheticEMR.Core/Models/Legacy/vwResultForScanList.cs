using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwResultForScanList
{
    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    public long? conID { get; set; }

    [StringLength(350)]
    public string? CLASS { get; set; }
}
