using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInvestigateArchive")]
public partial class hInvestigateArchive
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(2000)]
    public string? investigate { get; set; }

    [StringLength(2000)]
    public string? invResult { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }
}
