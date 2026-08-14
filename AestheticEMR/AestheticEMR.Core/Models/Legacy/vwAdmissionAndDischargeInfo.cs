using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAdmissionAndDischargeInfo
{
    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    public int? NumDays { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;
}
