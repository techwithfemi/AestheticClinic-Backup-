using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hDialysisDetails2")]
public partial class hDialysisDetails2
{
    public long SNo { get; set; }

    public long? SNoID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NA { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? K { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CL { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HCo3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Urea { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Creatininf { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DialType { get; set; }
}
