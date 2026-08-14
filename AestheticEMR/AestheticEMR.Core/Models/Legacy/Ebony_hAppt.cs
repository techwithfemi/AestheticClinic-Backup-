using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Ebony_hAppt
{
    public long ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinicType { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }
}
