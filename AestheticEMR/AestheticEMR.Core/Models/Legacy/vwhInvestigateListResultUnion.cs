using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultUnion
{
    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(301)]
    public string fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }
}
