using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Service
{
    public long? AcctID { get; set; }

    [StringLength(50)]
    public string? ServCode { get; set; }

    [Column("Service")]
    [StringLength(500)]
    public string? Service1 { get; set; }

    [StringLength(500)]
    public string? Class { get; set; }

    [StringLength(500)]
    public string? Category { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public bool? isHeading { get; set; }

    public long? AcctIDVal { get; set; }
}
