using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInvResultDetailsScan")]
public partial class hInvResultDetailsScan
{
    public long ID { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string LABNO { get; set; } = null!;

    [StringLength(350)]
    [Unicode(false)]
    public string? DESCRIPTION { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? RESULT { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? CLASS { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? RANGE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? REMARKS { get; set; }

    public long? SNoID { get; set; }
}
