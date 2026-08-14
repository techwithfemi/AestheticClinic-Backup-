using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwlabLabel
{
    public long IndexNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TagName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string lblDesc { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Range { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Units { get; set; } = null!;

    public long? SubClassID { get; set; }

    public long SNo { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? TagNo { get; set; }

    [StringLength(520)]
    [Unicode(false)]
    public string ClassName { get; set; } = null!;

    [StringLength(520)]
    [Unicode(false)]
    public string SubClassName { get; set; } = null!;

    [StringLength(400)]
    [Unicode(false)]
    public string? TagValue { get; set; }

    [StringLength(558)]
    [Unicode(false)]
    public string? TagValue2 { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Sample { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Reagent { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    public int HeaderIndexNo { get; set; }
}
