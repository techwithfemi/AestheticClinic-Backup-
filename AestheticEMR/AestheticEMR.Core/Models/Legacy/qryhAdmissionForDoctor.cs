using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAdmissionForDoctor
{
    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public bool? isDischarged { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
