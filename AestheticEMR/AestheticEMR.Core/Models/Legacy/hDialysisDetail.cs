using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hDialysisDetail
{
    public long Sno { get; set; }

    public long? SNoID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DialTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BP { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Pulse { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BFR { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UFR { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NP { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VP { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? IVF { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HEPperHR { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
