using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingVisitsAdmission2
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }
}
