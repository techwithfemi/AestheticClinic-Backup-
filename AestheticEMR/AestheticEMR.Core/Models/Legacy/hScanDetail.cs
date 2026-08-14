using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hScanDetail
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

    public string? RESULT { get; set; }

    [StringLength(350)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    public string? CLASS { get; set; }

    [StringLength(350)]
    public string? RANGE { get; set; }

    [StringLength(3050)]
    public string? REMARKS { get; set; }

    public long? ConID { get; set; }
}
