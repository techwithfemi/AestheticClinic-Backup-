using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHDiagnosis
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string disease { get; set; } = null!;

    [StringLength(50)]
    public string? ConID { get; set; }
}
